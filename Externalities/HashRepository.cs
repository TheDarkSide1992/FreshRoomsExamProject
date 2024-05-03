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
    
    public bool CreateGuid(string newGuid, string accperm)
    {
        const string sql = $@"INSERT INTO freshrooms.accountcode (guid, accperm, isUsed)
        VALUES (@newGuid, @accperm, false);";
        using (var connection = _dataSource.OpenConnection())
        {
            return connection.Execute(sql, new { newGuid, accperm } ) == 1;
        }
    }

    public bool CheckIfGuidExist(string newGuid)
    {
        const string sql = $@"SELECT COUNT(guid) from freshrooms.accountcode WHERE guid = @newGuid;";
        using (var connection = _dataSource.OpenConnection())
        {
            return connection.ExecuteScalar<int>(sql, new { newGuid } ) > 0;
        }
    }
    
    public int GetIdFromGuid(string guid)
    {
        const string sql = $@"SELECT id from freshrooms.accountcode WHERE guid = @guid;";
        
        using (var connection = _dataSource.OpenConnection())
        {
            return connection.QueryFirst<int>(sql, new { guid } );
        }
        
    }

    
    public void CreatePasswordHash(int userId, string hash, string salt, string algorithm)
    {
        const string sql = $@"
        INSERT INTO freshrooms.password_hash (user_id, hash, salt, algorithm)
        VALUES (@userId, @hash, @salt, @algorithm)
";
        using var connection = _dataSource.OpenConnection();
        connection.Execute(sql, new { userId, hash, salt, algorithm });
    }

    public bool UpdatePasswordHash(int userId, string hash, string salt, string algorithm)
    {
        const string sql = $@"
        UPDATE freshrooms.password_hash
        SET hash = @hash, salt = @salt, algorithm = @algorithm
        WHERE user_id = @userId
";
        using var connection = _dataSource.OpenConnection();
        return connection.Execute(sql, new { userId, hash, salt, algorithm }) == 1;
    }
    
    public HashModel GetByEmail(string email)
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