using Dapper;
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
                Console.WriteLine(oldData.timestamp);
                Console.WriteLine(oldData.sensorId);
                Console.WriteLine(oldData.Humidity);
                Console.WriteLine(oldData.CO2);
                Console.WriteLine(oldData.Temperature);
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
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.InnerException);
                throw new Exception("failed to save old data");
            }
        }
    }
}

