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

    public void CreateDevices(IEnumerable<DeviceModel> deviceList, int roomId)
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
    
}

