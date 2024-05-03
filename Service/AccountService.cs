using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Infastructure;
using Infastructure.DataModels;

namespace Service;

public class AccountService
{
    private readonly HashRepository _hashRepository;
    private readonly AccountRepository _accountRepository;


    public AccountService(AccountRepository accountRepository, HashRepository hashRepository)
    {
        _accountRepository = accountRepository;
        _hashRepository = hashRepository;
    }

    public string createAccountCode()
    {
        Guid guid;
        while (true)
        {
            guid = Guid.NewGuid();
            if (!_hashRepository.CheckIfGuidExist(guid.ToString()))
            {
                _hashRepository.CreateGuid(guid.ToString(), "user");
                return guid.ToString();
            }
        }
    }

    public User CreateUser(string userDisplayName, string userEmail, string password, string guid)
    {
        int userId = _hashRepository.GetIdFromGuid(guid);
        Console.WriteLine("first succes");
        if (userId != null && userId > 0)
        {
            var hashAlgorithm = PasswordHashAlgorithm.Create();
            var salt = hashAlgorithm.GenerateSalt();
            var hash = hashAlgorithm.HashPassword(password, salt);

            var user = _accountRepository.CreateUser(userId, userDisplayName, userEmail, false);
            _hashRepository.CreatePasswordHash(userId, hash, salt, hashAlgorithm.GetName());
            return user;
        }

        return null;
    }

    public User? Login(string email, string password)
    {
        var passwordHash = _hashRepository.GetByEmail(email);
        var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm);
        var isValid = hashAlgorithm.VerifyHashedPassword(password, passwordHash.Hash, passwordHash.Salt);
        if (isValid) return _accountRepository.GetById(passwordHash.id);

        return null;
    }

    public User? FindUserfromId(int id)
    {
        try
        {
            var isDeleted = _accountRepository.CheckIfUserIsDeleted(id);
            if (isDeleted == null)
            {
                return _accountRepository.GetById(id);
            }
        }
        catch (Exception e)
        {
            throw new AuthenticationException("The Account is deleted");
        }

        return null;
    }

    public AccountInfo? getAccountnfo(int id)
    {
        try
        {
            AccountInfo acInfo = _accountRepository.getAccountIngo(id);

            //THIS IS MOCK DATA
           /* AccountInfo acInfo = new AccountInfo()
            {
                realname = "Json Object",
                city = "Ram City",
                email = "IamJsoN@object.dev",
            };*/

            if (acInfo.city == null) acInfo.city = "N/A";
        }
        catch (Exception e)
        {
            throw new AuthenticationException("The Account is deleted or does not exist");
        }

        return null;
    }
}