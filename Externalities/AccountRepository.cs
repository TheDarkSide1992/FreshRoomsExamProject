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

    
   
    
    public User CreateUser(int id, string userDisplayName, string userEmail, bool isDeleted)
    {
        var sql =
            $@"INSERT INTO freshrooms.users (userId, name, email, isDeleted) VALUES(@id, @userDisplayName, @userEmail, @isDeleted) RETURNING 
        userId as {nameof(User.userId)}, 
        name as {nameof(User.userDisplayName)},
        email as {nameof(User.userEmail)};";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirst<User>(sql, new { id, userDisplayName, userEmail, isDeleted });

            }
            catch (Exception e)
            {
                throw new Exception("Could not create user");
            }
        }
    } 
}