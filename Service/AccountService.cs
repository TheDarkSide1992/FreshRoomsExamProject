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
            var userexists = _accountRepository.CheckIfUserIsDeleted(id);
            Console.WriteLine(userexists);
            if (userexists == 0)
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
            return _accountRepository.getAccountIngo(id);
        }
        catch (Exception e)
        {
            throw new AuthenticationException("The Account is deleted or does not exist");
        }
    }

    public bool changeAccountInfo(int userInfoUserId, string? dtoNewNameDto, string? dtoNewEmailDto, string? dtoNewCityDto, string? dtoNewPasswordDto)
    {
        bool couldUpdate = false;
        if (dtoNewNameDto != "N/A")
        {
            _accountRepository.updateName(userInfoUserId, dtoNewNameDto);
            couldUpdate = true;

        }        
        if (dtoNewEmailDto != "N/A")
        {
            _accountRepository.updateEmail(userInfoUserId, dtoNewEmailDto);
            couldUpdate = true;

        }        
        if (dtoNewCityDto != "N/A")
        {
            _accountRepository.updateCity(userInfoUserId, dtoNewCityDto);
            couldUpdate = true;

        }        
        if (dtoNewPasswordDto != "N/A")
        {
            //TODO HASH PASSWROD
            var hashAlgorithm = PasswordHashAlgorithm.Create();
            var salt = hashAlgorithm.GenerateSalt();
            var hash = hashAlgorithm.HashPassword(dtoNewPasswordDto, salt);
            _hashRepository.UpdatePasswordHash(userInfoUserId, hash, salt, hashAlgorithm.GetName());
            
            couldUpdate = true;
        }
        
        return couldUpdate;
    }
}