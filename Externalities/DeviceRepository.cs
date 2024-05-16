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

    public IEnumerable<BasicRoomStatus> getMotorsForRoom()
    {
        IEnumerable<int> roomIdList = null;
        List<BasicRoomStatus> roomStatusList = null;
        const string sql = $@"SELECT motorstatus.motorId as {nameof(MotorModel.motorId)}, motorstatus.isOpen as {nameof(MotorModel.isOpen)}, motorstatus.isDisabled as {nameof(MotorModel.isDisabled)}
                    FROM freshrooms.motorstatus join freshrooms.devices f on motorstatus.motorId = devices.deviceId where roomId = @room";

        const string sql2 = @"SELECT roomId as int FROM freshrooms.room";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                roomIdList = conn.Query<int>(sql2);
            }
            catch (Exception e)
            {
                throw new Exception("Could not get id's for rooms");
            }

            if (roomIdList != null)
            {
                foreach (var room in roomIdList)
                {
                    try
                    {
                        string output = "";
                        BasicRoomStatus roomStatus = new BasicRoomStatus();
                        IEnumerable<MotorModel> motorList = conn.Query<MotorModel>(sql, new { room });
                        foreach (var motor in motorList)
                        {
                            if (motor.isOpen)
                            {
                                output = "Open";
                            }
                            else if(!motor.isOpen && output != "")
                            {
                                roomStatus.basicWindowStatus = "Mixed";
                                break;
                            }
                            else if(!motor.isOpen)
                            {
                                output = "Closed";
                            }
                            roomStatus.basicWindowStatus = output;
                        }
                        roomStatusList.Add(roomStatus);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Could not get motor status for rooms");
                    }
                }
            }
            return roomStatusList;
        }
    }
}

