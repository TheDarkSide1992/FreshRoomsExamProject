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
    
    public bool CreateRoomConfig(int roomId)
    {
        
        var sql =
            $@"INSERT INTO freshrooms.roomConfig(roomid, mintemparature, maxtemparature, minhumidity, maxhumidity, minaq, maxaq) VALUES (@roomId, 17,25,40,60,0.4,1)";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Execute(sql, new { roomId }) == 1;
            }
            catch (Exception e)
            {
                throw new Exception("Could not create roomConfig");
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

    public bool DeleteRoomConfig(int roomId)
    {
        var sql = $@"DELETE FROM freshrooms.roomConfig WHERE roomId = @roomId;";
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Execute(sql, new { roomId }) == 1;
            }
            catch (Exception e)
            {
                throw new Exception("Could not delete roomConfig");
            }
        }
    }

    public bool DeleteRoom(int roomId)
    {
        var sql = $@"DELETE FROM freshrooms.rooms WHERE roomId = @roomId;";
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Execute(sql, new { roomId }) == 1;
            }
            catch (Exception e)
            {
                throw new Exception("Could not delete room");
            }
        }
    }
}