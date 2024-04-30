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

    public bool CheckIfGuidExist(string guid)
    {
        const string sql = $@"SELECT COUNT(*) guid from freshrooms.accountcode WHERE guid = @guid;";
        using var connection = _dataSource.OpenConnection();
        int userCount = (int) connection.ExecuteScalar(sql, new { guid } );
        return userCount > 0;
    }
    
    public int GetIdFromGuid(string guid)
    {
        const string sql = $@"SELECT guid from freshrooms.accountcode WHERE guid = @guid RETURNING id as int;";
        
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst(sql, new { guid } );
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
}