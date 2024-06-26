using Dapper;
using Npgsql;

namespace Infastructure;

public class HashRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public HashRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    /**
     *This is used to create a accountcode in the DB
     * Account codes is needed to register a new user
     */
    public bool createGuid(string newGuid, string accperm)
    {
        const string sql = $@"INSERT INTO freshrooms.accountcode (guid, accperm, isUsed)
        VALUES (@newGuid, @accperm, false);";
        using (var connection = _dataSource.OpenConnection())
        {
            return connection.Execute(sql, new { newGuid, accperm } ) == 1;
        }
    }

    /**
     *This is used to check if a specific Guid exist in the DB table accountcode.
     */
    public bool checkIfGuidExist(string newGuid)
    {
        const string sql = $@"SELECT COUNT(guid) from freshrooms.accountcode WHERE guid = @newGuid;";
        using (var connection = _dataSource.OpenConnection())
        {
            return connection.ExecuteScalar<int>(sql, new { newGuid } ) > 0;
        }
    }
    
    /**
     *This is used to get the autogenerated id related to a specific Guid in the DB
     */
    public int getIdFromGuid(string guid)
    {
        const string sql = $@"SELECT id from freshrooms.accountcode WHERE guid = @guid;";
        
        using (var connection = _dataSource.OpenConnection())
        {
            return connection.QueryFirst<int>(sql, new { guid } );
        }
        
    }

    /**
     *This is used to store a Hashed password in the db with its salt, algorithm and related userid
     */
    public void createPasswordHash(int userId, string hash, string salt, string algorithm)
    {
        const string sql = $@"
        INSERT INTO freshrooms.password_hash (user_id, hash, salt, algorithm)
        VALUES (@userId, @hash, @salt, @algorithm)
";
        using var connection = _dataSource.OpenConnection();
        connection.Execute(sql, new { userId, hash, salt, algorithm });
    }

    /**
     *This is used to update a password hash in the DB
     */
    public bool updatePasswordHash(int userId, string hash, string salt, string algorithm)
    {
        const string sql = $@"
        UPDATE freshrooms.password_hash
        SET hash = @hash, salt = @salt, algorithm = @algorithm
        WHERE user_id = @userId
";
        using var connection = _dataSource.OpenConnection();
        return connection.Execute(sql, new { userId, hash, salt, algorithm }) == 1;
    }
    
    /**
     *This is used to return a HashModel related to the user, by using their email as an identifier.
     */
    public HashModel getByEmail(string email)
    {
        const string sql = $@"
SELECT 
    user_id as {nameof(HashModel.id)},
    hash as {nameof(HashModel.Hash)},
    salt as {nameof(HashModel.Salt)},
    algorithm as {nameof(HashModel.Algorithm)}
FROM freshrooms.password_hash
JOIN freshrooms.users ON freshrooms.password_hash.user_id = freshrooms.users.userId
WHERE email = @email;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QuerySingle<HashModel>(sql, new { email });
    }
}