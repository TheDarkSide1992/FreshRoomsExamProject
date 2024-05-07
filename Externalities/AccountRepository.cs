using Dapper;
using Infastructure.DataModels;
using Npgsql;
using AccountInfo = Infastructure.DataModels.AccountInfo;

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


    public User? GetById(int id)
    {
        try
        {
            var sql = $@"select
                   userId as {nameof(User.userId)},
                   name as {nameof(User.userDisplayName)},
                   email as {nameof(User.userEmail)}
                   from freshrooms.users where userId = @id and isDeleted = false";
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirst<User>(sql, new { id });
        }
        catch (Exception e)
        {
            throw new Exception("failed to login");
        }
    }

    public int CheckIfUserIsDeleted(int id)
    {
        try
        {
            var sql = $@"select
                   userId as {nameof(User.userId)},
                   name as {nameof(User.userDisplayName)},
                   email as {nameof(User.userEmail)}
                   from freshrooms.users where userId = @id and isDeleted = true";
            using var connection = _dataSource.OpenConnection();
            return connection.ExecuteScalar<int>(sql, new { id });
        }
        catch (Exception e)
        {
            throw new Exception("failed to get user");
        }
    }

    public AccountInfo getAccountIngo(int id)
    {
        var sql = $@"select
                   name as {nameof(AccountInfo.realname)},
                   email as {nameof(AccountInfo.email)},
                  city as {nameof(AccountInfo.city)}

                from freshrooms.users where userId = @id and isDeleted = false";

        try
        {
            using (var connection = _dataSource.OpenConnection())
            {
                return connection.QueryFirst<AccountInfo>(sql, new { id });
            }
        }
        catch (Exception e)
        {
            throw new Exception("failed to get Account info");
        }
    }

    public string getCityFromUser(int id)
    {
        var sql = $@"select city from freshrooms.users where userId = @id";
        try
        {
            using (var connection = _dataSource.OpenConnection())
            {
                return connection.QueryFirst<string>(sql, new { id });
            }
        }
        catch (Exception e)
        {
            throw new Exception("failed to get city from account");
        }
    }
}