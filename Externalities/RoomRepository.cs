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
                throw new Exception("Could not get room Data, you donout");
            }
        }
        
        return new RoomConfigModel(){
            minTemparature = 12.0,
            maxTemparature = 22.0,
            maxHumidity = 25.5,
            minHumidity = 2.0,
            minAq = 1.0,
            maxAq = 2.0,
        };
    }
}