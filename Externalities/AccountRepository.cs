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

    public User CreateUser(string userDisplayName, string userEmail)
    {
        var sql =
            $@"INSERT INTO freshrooms.users (name, email) VALUES(@userDisplayName, @userEmail,@userBirthday, false) RETURNING 
        id as {nameof(User.userId)}, 
        name as {nameof(User.userDisplayName)},
        email as {nameof(User.userEmail)};";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirst<User>(sql, new { userDisplayName, userEmail });

            }
            catch (Exception e)
            {
                throw new Exception("Could now create user");
            }
        }
    } 
}