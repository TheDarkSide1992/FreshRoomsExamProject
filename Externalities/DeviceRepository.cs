using System.Data;
using Dapper;
using Infastructure.CostumExeptions;
using Infastructure.DataModels;
using Npgsql;

namespace Infastructure;

public class DeviceRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DeviceRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public bool VerifySensorGuid(string sensorGuid)
    {
        const string sql = $@"SELECT COUNT(deviceId) from freshrooms.devices WHERE deviceId = @sensorGuid;";
        using (var connection = _dataSource.OpenConnection())
        {
            return connection.ExecuteScalar<int>(sql, new { sensorGuid } ) > 0;
        }
    }

    public DeviceTypeModel createDeviceType(string deviceType)
    {
        const string sql =
            $@"INSERT INTO freshrooms.devicetypes(deviceTypeName) VALUES (@deviceType) RETURNING 
        deviceTypeId as {nameof(DeviceTypeModel.deviceTypeId)},
        deviceTypeName as {nameof(DeviceTypeModel.deviceTypeName)};";
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirst<DeviceTypeModel>(sql, new { deviceType });
            }
            catch (Exception e)
            {
                throw new Exception("Could not create devicetype");
            }
        }
    }

    public bool deleteDeviceType(int deviceTypeId)
    {
        const string sql =
            $@"UPDATE freshrooms.devicetypes SET isDeleted = false WHERE deviceTypeId = @deviceTypeid";
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Execute(sql, new { deviceTypeId }) == 1;
            }
            catch (Exception e)
            {
                throw new Exception("Could not delete devicetype");
            }
        }
    }

    public IEnumerable<DeviceTypeModel> getDeviceTypes()
    {
        
        var sql = $@"
        select 
             deviceTypeId as {nameof(DeviceTypeModel.deviceTypeId)},
             deviceTypeName as {nameof(DeviceTypeModel.deviceTypeName)}
        from freshrooms.devicetypes WHERE isDeleted = false;
        ";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<DeviceTypeModel>(sql, new { });
        }
    }

    public void UpdateDevices(IEnumerable<DeviceModel> deviceList, int roomId)
    {
        using (var conn = _dataSource.OpenConnection())
        {
            const string sql =
                $@"UPDATE freshrooms.devices SET roomId = @roomId, deviceType = @tempDeviceType WHERE deviceId = @tempGuid";
            
            var transaction = conn.BeginTransaction();
            try
            {
                foreach (var device in deviceList)
                {
                    conn.Query(sql, new { roomId, tempGuid = device.sensorGuid, tempDeviceType = device.deviceTypeName });
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception("Could not create devices");
            }
        }
    }

    public bool DeleteRoomIdOnDevices(int roomId)
    {
        const string sql =
            $@"UPDATE freshrooms.devices SET roomId = null WHERE roomId = @roomId";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Execute(sql, new { roomId }) != 0;
            }
            catch (Exception e)
            {
                throw new Exception("Could not delete device to room relation");
            }
        }
    }

    public List<BasicDeviceDModel> GetBasicDeviceData()
    {
        const string sql = $@"select roomid as {nameof(BasicDeviceDModel.roomId)}, temp as {nameof(BasicDeviceDModel.cTemp)}, 
       hum as {nameof(BasicDeviceDModel.cHum)}, aq as {nameof(BasicDeviceDModel.cAq)}, isopen as {nameof(BasicDeviceDModel.isOpen)},
       devicetype as {nameof(BasicDeviceDModel.deviceType)}
                     from freshrooms.devices
                               left  join freshrooms.motorstatus s on deviceid = s.motorid
                               left  join freshrooms.devicedata m on deviceid = m.sensorid
                        where roomid is not null;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<BasicDeviceDModel>(sql) as List<BasicDeviceDModel>;
        }
    }

    public SensorModel createOrUpdateData(SensorModel sensorModel)
    {
        var sql = $@"insert into freshrooms.devicedata (sensorId, temp, hum, aq, timestamp) values (@sensorId,@temp,@hum,@aq,@timestamp)
                        on conflict(sensorId)
                        do update set temp = @temp, hum = @hum, aq = @aq, timestamp = @timestamp;";
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {

                conn.Execute(sql, new
                {
                    sensorModel.sensorId,
                    temp = sensorModel.Temperature,
                    hum = sensorModel.Humidity,
                    aq = sensorModel.CO2,
                    timestamp = DateTime.Now
                });
                return sensorModel;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to save sensor data");
            }
        }
    }

    public bool createOrUpdateMoterStatus(MotorModel motorModel)
    {
        var sql = $@"insert into freshrooms.motorstatus (motorId, isOpen, isDisabled) values (@motorId,@isOpen,@isDisabled)
                        on conflict(motorId)
                        do update set isOpen = @isOpen;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                conn.Execute(sql, new
                {
                    motorModel.MotorId,
                    motorModel.isOpen,
                    isDisabled = false
                });
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to save window status");
            }
        }
    }

    public void saveOldData(string sensorId)
    {
        var selectOldData = $@"select sensorId, temp as {nameof(SensorModel.Temperature)}, hum as {nameof(SensorModel.Humidity)}, aq as {nameof(SensorModel.CO2)}, timestamp from freshrooms.devicedata where sensorId = @sensorId";
        var save =
            $@"insert into freshrooms.historicdata (sensorId, temp, hum, aq, timestamp) values (@sensorId,@temp,@hum,@aq,@timestamp);";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                var oldData = conn.QueryFirst<SensorModel>(selectOldData, new { sensorId });
                conn.Execute(save,
                    new
                    {
                        oldData.sensorId, hum = oldData.Humidity,
                        temp = oldData.Temperature, aq = oldData.CO2,
                        oldData.timestamp
                    });
            }
            catch (Exception e)
            {
                throw new Exception("failed to save old data");
            }
        }
    }

    public RoomAvrageSensorData getAvrageSensordataforRoom(string sensorId)
    {
        var getRoomid = $@"select roomid from freshrooms.devices where deviceid = @sensorId";
        var getAvrage = $@"select avg(hum) as {nameof(RoomAvrageSensorData.Humidity)}, avg(temp) as {nameof(RoomAvrageSensorData.Temperature)}, avg(aq) as {nameof(RoomAvrageSensorData.CO2)}, d.roomid as {nameof(RoomAvrageSensorData.roomId)} from freshrooms.devicedata join freshrooms.devices d on d.deviceid = devicedata.sensorid where d.roomid = @roomid
                            group by d.roomid;";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                var roomid = conn.QueryFirst<int>(getRoomid, new { sensorId });
                return conn.QueryFirst<RoomAvrageSensorData>(getAvrage, new { roomid });
            }
            catch (Exception e)
            {
                throw new DataNotFoundExeption("Failed to get average data for room");
            }
        }
    }

    public IEnumerable<MotorModel> getMotersForRoom(int roomId)
    {
        var sql = $@"select * from freshrooms.motorstatus join freshrooms.devices d on d.deviceid = motorstatus.motorid where d.roomid = @roomId and isDisabled = @isdisabled;";
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Query<MotorModel>(sql, new { roomId, isDisabled = false });
            }
            catch (Exception e)
            {
                throw new DataNotFoundExeption("failed to get window motors for room");
            }
        }
    }

    public int getRoomIdFromDeviceId(string id)
    {
        var sql = $@"select roomid from freshrooms.devices where deviceid = @id;";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirst<int>(sql, new { id });
            }
            catch (Exception e)
            {
                throw new DataNotFoundExeption("Failed to get room id");
            }
            
        }
    }

    public List<SensorModel> getSensorsForRoom(int roomid)
    {
        var sql = $@"select sensorid as {nameof(SensorModel.sensorId)}, hum as {nameof(SensorModel.Humidity)}, temp as {nameof(SensorModel.Temperature)}, aq as {nameof(SensorModel.CO2)} from freshrooms.devicedata join freshrooms.devices d on d.deviceid = devicedata.sensorid where d.roomid = @roomid;";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return (List<SensorModel>)conn.Query<SensorModel>(sql, new { roomid });
            }
            catch (Exception e)
            {
                throw new DataNotFoundExeption("Failed to get sensors for the room");
            }
        }
    }

    public bool DeleteCurrentDataForRoom(int roomId)
    {
        const string sql =
            $@"DELETE FROM freshrooms.devicedata
               USING freshrooms.devices
               WHERE devices.deviceid = devicedata.sensorId and devices.roomId = @roomId;";
        
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Execute(sql, new { roomId }) != 0;
            }
            catch (Exception e)
            {
                throw new Exception("Could not delete current device data for room");
            }
        }
    }

    public bool DeleteHistoricDataForRoom(int roomId)
    {
        const string sql =
            $@"DELETE FROM freshrooms.historicData
               USING freshrooms.devices
               WHERE devices.deviceid = historicData.sensorId and devices.roomId = @roomId;";
        
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Execute(sql, new { roomId }) != 0;
            }
            catch (Exception e)
            {
                throw new Exception("Could not delete historic device data for room");
            }
        }
    }
}

