using Dapper;
using Infastructure.CostumExeptions;
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

    /**
     *This is used to create a room in the DB
     */
    public RoomModel createRoom(string name, int createdBy)
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
    
    /**
     *This is used to create a roomConfig related to a specific room in the DB
     */
    public bool createRoomConfig(int roomId)
    {
        
        var sql =
            $@"INSERT INTO freshrooms.roomConfig(roomid, mintemparature, maxtemparature, minhumidity, maxhumidity, minaq, maxaq) VALUES (@roomId, 20,26,30,60,2,5)";

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
    
    /**
     *This returns a RoomConfigModel for a specific room
     */
    public RoomConfigModel getRoomPreferencesConfiguration(int dtoRoomId)
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

    /**
     *This updates a roomconfig in the db, for a specific room.
     */
    public RoomConfigModel updateRoomPreferencesConfiguration(int dtoRoomId, double dtoUpdatedMinTemperature, double dtoUpdatedMaxTemperature, 
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

    /**
     *This deletes a roomconfig in the db for a specific room
     */
    public bool deleteRoomConfig(int roomId)
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

    /**
     *This deletes a specific room in the db.
     */
    public bool deleteRoom(int roomId)
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

    /**
     *This returns a list of BasicRoomSettingModel
     */
    public IEnumerable<BasicRoomSettingModel> getAllRoomSettings()
    {
        var sql = $@"select r.roomid,
       roomname as {nameof(BasicRoomSettingModel.roomName)},
       c.mintemparature as {nameof(BasicRoomSettingModel.minTemparature)},
       c.maxtemparature as {nameof(BasicRoomSettingModel.maxTemparature)},
       c.minhumidity as {nameof(BasicRoomSettingModel.minHumidity)},
       c.maxhumidity as {nameof(BasicRoomSettingModel.maxHumidity)},
       c.minaq as {nameof(BasicRoomSettingModel.minAq)},
       c.maxaq as {nameof(BasicRoomSettingModel.maxAq)}
from freshrooms.rooms r
         join freshrooms.roomconfig c on r.roomid = c.roomid;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.Query<BasicRoomSettingModel>(sql);
            }
            catch (Exception e)
            {
                throw new Exception("Could not get room config");
            }
        }
        
    }

    /**
     * This returns the roomName from the db for a specific room
     */
    public string getRoomName(int roomid)
    {
        var sql = $@"select roomname from freshrooms.rooms where roomid = @roomid";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirst<string>(sql, new { roomid });
            }
            catch (Exception e)
            {
                throw new DataNotFoundExeption("failed to get room name");
            }
        }
    }
}