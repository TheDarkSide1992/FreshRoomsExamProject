using Dapper;
using Infastructure.DataModels;
using Npgsql;

namespace Infastructure;

public class RoomRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public RoomRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public RoomModel CreateRoom(string name)
    {
        
        var sql =
            $@"INSERT INTO freshrooms.rooms (roomName) VALUES(@name) RETURNING 
        roomId as {nameof(RoomModel.roomId)}, 
        roomName as {nameof(RoomModel.name)};";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirst<RoomModel>(sql, new { name });
            }
            catch (Exception e)
            {
                throw new Exception("Could not create room");
            }
        }
    }

    public RoomConfigModel getRoomPrefrencesConfiguration(int userInfoUserId, int dtoRoomId)
    {
        var sql = $@"SELECT 
    mintemparature as {nameof(RoomConfigModel.minTemparature)}, 
    maxtemparature as {nameof(RoomConfigModel.maxTemparature)},
    minhumidity as {nameof(RoomConfigModel.minHumidity)},
    maxhumidity as {nameof(RoomConfigModel.maxHumidity)},
    minaq as {nameof(RoomConfigModel.minAq)},
    maxaq as {nameof(RoomConfigModel.maxAq)}
    FROM freshrooms.roomConfig WHERE roomid = @dtoRoomId;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QuerySingle<RoomConfigModel>(sql, new { dtoRoomId });
            }
            catch (Exception e)
            {
                throw new Exception("Could not get room Data.");
            }
        }
    }


    public RoomConfigModel updateRoomPrefrencesConfiguration(int dtoRoomId, double dtoUpdatedMinTemperature, double dtoUpdatedMaxTemperature, 
        double dtoUpdatedMinHumidity, double dtoUpdatedMaxHumidity, double dtoUpdatedMinAq, double dtoUpdatedMaxAq)
    {
        var sql = $@"UPDATE freshrooms.roomConfig
SET mintemparature = @dtoUpdatedMinTemperature, maxtemparature = @dtoUpdatedMaxTemperature, 
    minhumidity = @dtoUpdatedMinHumidity, maxhumidity = @dtoUpdatedMaxHumidity, minaq = @dtoUpdatedMinAq, 
    maxaq = @dtoUpdatedMaxAq WHERE roomid = @dtoRoomId

    RETURNING mintemparature as {nameof(RoomConfigModel.minTemparature)}, 
    maxtemparature as {nameof(RoomConfigModel.maxTemparature)},
    minhumidity as {nameof(RoomConfigModel.minHumidity)},
    maxhumidity as {nameof(RoomConfigModel.maxHumidity)},
    minaq as {nameof(RoomConfigModel.minAq)},
    maxaq as {nameof(RoomConfigModel.maxAq)}
;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QuerySingle<RoomConfigModel>(sql, new {dtoUpdatedMinTemperature, dtoUpdatedMaxTemperature,dtoUpdatedMinHumidity, dtoUpdatedMaxHumidity, dtoUpdatedMinAq, dtoUpdatedMaxAq, dtoRoomId });
            }
            catch (Exception e)
            {
                throw new Exception("Could not update room Data.");
            }
        }
    }

    public RoomDataModel getGraphData(int roomid, DateTime startInterval, DateTime endInterval)
    {
        //TODO Make SQL string for getting average temp, hum and aq in time interval based on room-id 
        var sql = $@"select avg(hum) as {nameof(RoomDataModel.avgHumidity)}, avg(temp) as {nameof(RoomDataModel.avgTemperature)}, 
       avg(aq) as {nameof(RoomDataModel.avgCO2)}, 
       d.roomid as {nameof(RoomDataModel.roomId)} from freshrooms.devicedata join freshrooms.devices d on d.deviceid = devicedata.sensorid 
       where d.roomid = @roomid group by d.roomid and timestamp >= @startInterval and timestamp <= @endInterval;";


        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QuerySingle<RoomDataModel>(sql, new { roomid, startInterval, endInterval});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}