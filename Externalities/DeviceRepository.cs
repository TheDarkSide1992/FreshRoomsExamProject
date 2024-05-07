using Dapper;
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
}