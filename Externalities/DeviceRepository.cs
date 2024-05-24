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
    
    /*
     * This method takes in a IEnumerable<DeviceModel> object and an int to update devices for the room that has the given int as it's id
     */
    public void updateDevices(IEnumerable<DeviceModel> deviceList, int roomId)
    {
        using (var conn = _dataSource.OpenConnection())
        {
            const string sql = $@"insert into freshrooms.devices (deviceId, roomId, deviceType) values (@tempGuid, @roomId, @tempDeviceType)
                        on conflict(deviceId)
                        do UPDATE SET roomId = @roomId, deviceType = @tempDeviceType;";
            
            var transaction = conn.BeginTransaction();
            try
            {
                foreach (var device in deviceList)
                {
                    conn.Query(sql, new { roomId, tempGuid = device.deviceGuid, tempDeviceType = device.deviceTypeName });
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

    /*
     * This method takes in an int which is the id of a room and removes it from devices where it appears 
     */
    public bool deleteRoomIdOnDevices(int roomId)
    {
        const string sql =
            $@"DELETE FROM freshrooms.devices WHERE roomId = @roomId";
        
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
    
    
    /*
     * this method deletes a motor's status where the given int is present as the roomId
     */
    public bool deleteMotorStatus(int roomId)
    {
        const string sql =
            $@"DELETE FROM freshrooms.motorstatus
               USING freshrooms.devices
               WHERE devices.deviceid = motorstatus.motorId and devices.roomId = @roomId;";
        
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Execute(sql, new { roomId }) != 0;
            }
            catch (Exception e)
            {
                throw new Exception("Could not delete motors status for devices in room");
            }
        }
    }
    
    /*
     * this method returns a list of BasicDeviceDModel
     */
    public List<BasicDeviceDModel> getBasicDeviceData()
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

    /*
     * this method takes in a SensorModel object and uses an upsert to either create or update the data in the database table
     */
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
                    temp = sensorModel.temperature,
                    hum = sensorModel.humidity,
                    aq = sensorModel.co2,
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
    
    /*
     * This method takes in a MotorModel object to update the motor's isOpen in the database table 
     */
    public void updateMoterStatusMQTT(MotorModel motorModel)
    {
        var sql = $@"update freshrooms.motorstatus set isOpen = @isOpen where motorid = @motorId;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                conn.Execute(sql, new
                {
                    motorModel.motorId,
                    motorModel.isOpen,
                });
            }
            catch (Exception e)
            {
                throw new Exception("Failed to save window status");
            }
        }
    }
    
    /*
     * this method takes in an IEnumerable<DeviceModel> object and uses a transaction to create all motors in the list in the database table
     */
    public bool createMoterStatusList(IEnumerable<DeviceModel> motorModels)
    {
        var sql = $@"insert into freshrooms.motorstatus (motorId, isOpen, isDisabled) values (@motorId,@isOpen,@isDisabled)
                        on conflict(motorId)
                        do update set isOpen = @isOpen;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            var transaction = conn.BeginTransaction();
            try
            {
                foreach (var motor in motorModels)
                {
                    if (motor.deviceTypeName == "Window Motor")
                    {
                        conn.Query(sql, new { motorId = motor.deviceGuid, isOpen = false,  isDisabled = false});
                    }
                    
                }
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to create window status");
            }
        }
    }
    
    /*
     * This method takes in a MotorModel object to update the motor's isOpen and isDisabled in the database table
     */
    public void updateMotorModelWithUsersInput(MotorModel motorModel)
    {
        var save = $@"update freshrooms.motorstatus set isOpen = @isOpen, isdisabled = @isDisabled where motorid = @motorId;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                conn.Execute(save, new
                {
                    motorModel.motorId,
                    motorModel.isOpen,
                    motorModel.isDisabled
                });
            }
            catch (Exception e)
            {
                throw new Exception("Failed to save window status");
            }
        }
    }
    
    /*
     * This method takes in a roomId and two bools object to update the motor's isOpen and isDisabled in the database table
     */
    public IEnumerable<MotorModel> updateAllMotersInARoom(int roomid, bool open, bool isDisabled)
    {
        var sql = $@"update freshrooms.motorstatus set isopen = @isopen, isdisabled = @isDisabled from freshrooms.devices where freshrooms.devices.deviceid = freshrooms.motorstatus.motorid and freshrooms.devices.roomid = @roomid
                        returning
                        motorid as {nameof(MotorModel.motorId)},
                        isopen as {nameof(MotorModel.isOpen)},
                        isdisabled as {nameof(MotorModel.isDisabled)};";
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Query<MotorModel>(sql, new { roomid, isopen = open, isDisabled });
            }
            catch (Exception e)
            {
                throw new MotorUpdateExeption("Failed to update window status");
            }
        }
    }

    /*
     * this method returns a bool true if a sensor's data exits and false if it does not exist
     */
    public bool checkIfSensorDataExist(string sensorGuid)
    {
        var sql = $@"SELECT count(*) FROM freshrooms.devicedata WHERE sensorid = @sensorGuid;";
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.ExecuteScalar<int>(sql, new { sensorGuid }) != 0;
            }
            catch (Exception e)
            {
                throw new DataNotFoundExeption("No sensor found");
            }
        }
    }

    /*
     * this method takes in a sensorId and finds the old data from the devicedata table and saves it in the historicdata table
     */
    public void saveOldData(string sensorId)
    {
        var selectOldData = $@"select sensorId, temp as {nameof(SensorModel.temperature)}, hum as {nameof(SensorModel.humidity)}, aq as {nameof(SensorModel.co2)}, timestamp from freshrooms.devicedata where sensorId = @sensorId";
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
                        oldData.sensorId, hum = oldData.humidity,
                        temp = oldData.temperature, aq = oldData.co2,
                        oldData.timestamp
                    });
            }
            catch (Exception e)
            {
                throw new Exception("failed to save old data");
            }
        }
    }
    
    /*
     * this method gets average sensor data for a room
     */
    public RoomAverageSensorData getAverageSensordataforRoom(string sensorId)
    {
        var getRoomid = $@"select roomid from freshrooms.devices where deviceid = @sensorId";
        var getAvrage = $@"select avg(hum) as {nameof(RoomAverageSensorData.Humidity)}, avg(temp) as {nameof(RoomAverageSensorData.Temperature)}, avg(aq) as {nameof(RoomAverageSensorData.CO2)}, d.roomid as {nameof(RoomAverageSensorData.roomId)} from freshrooms.devicedata join freshrooms.devices d on d.deviceid = devicedata.sensorid where d.roomid = @roomid
                            group by d.roomid;";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                var roomid = conn.QueryFirst<int>(getRoomid, new { sensorId });
                return conn.QueryFirst<RoomAverageSensorData>(getAvrage, new { roomid });
            }
            catch (Exception e)
            {
                throw new DataNotFoundExeption("Failed to get average data for room");
            }
        }
    }

    /*
     * This method gets all users from the given roomId parameters and returns a list of MotorModels
     */
    public IEnumerable<MotorModel> getMotorsForRoom(int roomId)
    {
        var sql = $@"select * from freshrooms.motorstatus join freshrooms.devices d on d.deviceid = motorstatus.motorid where d.roomid = @roomId";
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

    /*
     * this method takes in a string called id and uses it to get the id of the room the device is connected to and returns the room's id which is an int
     */
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

    /*
     * this method takes in a int called roomid and returns a list of SensorModel objects which are the sensors that are connected to the room
     */
    public List<SensorModel> getSensorsForRoom(int roomid)
    {
        var sql = $@"select sensorid as {nameof(SensorModel.sensorId)}, hum as {nameof(SensorModel.humidity)}, temp as {nameof(SensorModel.temperature)}, aq as {nameof(SensorModel.co2)} from freshrooms.devicedata join freshrooms.devices d on d.deviceid = devicedata.sensorid where d.roomid = @roomid;";

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

    /*
     * this method takes in an int called roomId and deletes data from the devicedata table where the given roomId is present
     */
    public bool deleteCurrentDataForRoom(int roomId)
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

    /*
     * this method takes in an int called roomId and deletes data from the devicedata table where the given roomId is present
     */
    public bool deleteHistoricDataForRoom(int roomId)
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

