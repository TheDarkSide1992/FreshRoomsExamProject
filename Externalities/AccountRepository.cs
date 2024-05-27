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

    /**
     * This method is used to create a user in the DB
     */
    public User createUser(int id, string userDisplayName, string userEmail, bool isDeleted)
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


    /**
     * This method is used to get a user object from the DB, by using the user id.
     */
    public User? getById(int id)
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

    /**
     * This is used to check if a user is deleted, by checking if it exist in the DB
     */
    public int checkIfUserIsDeleted(int id)
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

    /**
     *This is used to get account info for the user from DB
     */
    public AccountInfo getAccountInfo(int id)
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
                Console.WriteLine("runnign sql get account info"); //TODO Remowe Before deployment
                return connection.QueryFirst<AccountInfo>(sql, new { id });
            }
        }
        catch (Exception e)
        {
            throw new Exception("failed to get Account info");
        }
    }

    /**
     *This is used to get the city tied to the user from DB
     */
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

    /**
     *this is used to update the users name in the DB
     */
    public void updateName(int userInfoUserId, string? dtoNewNameDto)
    {
        var sql = $@"
        UPDATE freshrooms.users SET name = @dtoNewNameDto  WHERE userId = @userInfoUserId 
        ";

        try
        {
            using (var conn = _dataSource.OpenConnection())
            {
                Console.WriteLine("runnign sql update Name"); //TODO Remowe Before deployment
                conn.Execute(sql, new { dtoNewNameDto, userInfoUserId });
            }
        }
        catch (Exception e)
        {
            throw new Exception("failed update Account Name");
        }
    }

    /**
     *This is used to update the users email in the DB
     */
    public void updateEmail(int userInfoUserId, string? dtoNewEmailDto)
    {
        var sql = $@"
        UPDATE freshrooms.users SET email = @dtoNewEmailDto  WHERE userId = @userInfoUserId 
        ";
        try
        {
            using (var conn = _dataSource.OpenConnection())
            {
                Console.WriteLine("runnign sql update Email"); //TODO Remowe Before deployment
                conn.Execute(sql, new { dtoNewEmailDto, userInfoUserId });
            }
        }
        catch (Exception e)
        {
            throw new Exception("failed update Account Email");
        }
    }

    /**
     *This is used to update the city tied to the user in the DB
     */
    public void updateCity(int userInfoUserId, string? dtoNewCityDto)
    {
        var sql = $@"
        UPDATE freshrooms.users SET city = @dtoNewCityDto  WHERE userId = @userInfoUserId 
        ";

        try
        {
            using (var conn = _dataSource.OpenConnection())
            {
                Console.WriteLine("runnign sql update City"); //TODO Remowe Before deployment
                conn.Execute(sql, new { dtoNewCityDto, userInfoUserId });
            }
        }
        catch (Exception e)
        {
            throw new Exception("failed update Account City");
        }
    }

    /**
     *This is used to check in the DB, if the user is an Admin
     */
    public bool isAdmin(int userInfoUserId)
    {
        var sql = $@"SELECT id FROM freshrooms.accountcode WHERE id = @userInfoUserId AND accperm = 'admin'";

        try
        {
            using (var conn = _dataSource.OpenConnection())
            {
                int id = conn.QuerySingle<int>(sql, new { userInfoUserId });
                return id == userInfoUserId;
            }
        }
        catch (Exception e)
        {
            throw new Exception("You dont have Permission for this action");
        }

    }
}