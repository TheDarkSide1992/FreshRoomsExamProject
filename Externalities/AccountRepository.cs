using Dapper;
using Infastructure.DataModels;
using Npgsql;

namespace Infastructure;

public class AccountRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public AccountRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    
   
    
    public User CreateUser(int id, string userDisplayName, string userEmail)
    {
        var sql =
            $@"INSERT INTO freshrooms.users (id, name, email) VALUES(@id, @userDisplayName, @userEmail, false) RETURNING 
        id as {nameof(User.userId)}, 
        name as {nameof(User.userDisplayName)},
        email as {nameof(User.userEmail)};";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirst<User>(sql, new { id, userDisplayName, userEmail });

            }
            catch (Exception e)
            {
                throw new Exception("Could not create user");
            }
        }
    } 
}