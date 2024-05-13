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

    public RoomModel CreateRoom(string name, int createdBy)
    {
        
        var sql =
            $@"INSERT INTO freshrooms.rooms (roomName, userId) VALUES(@name, @createdBy) RETURNING 
        roomId as {nameof(RoomModel.roomId)}, 
        roomName as {nameof(RoomModel.name)},
        userId as {nameof(RoomModel.creatorId)};";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirst<RoomModel>(sql, new { name, createdBy });
            }
            catch (Exception e)
            {
                throw new Exception("Could not create room");
            }
        }
    }
    
    public IEnumerable<RoomModel> GetAllRooms()
    {
        
        var sql =
            $@"SELECT 
        roomId as {nameof(RoomModel.roomId)}, 
        roomName as {nameof(RoomModel.name)},
        userId as {nameof(RoomModel.creatorId)} 
            FROM freshrooms.rooms";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Query<RoomModel>(sql, new {});
            }
            catch (Exception e)
            {
                throw new Exception("Could not get rooms");
            }
        }
    }
}