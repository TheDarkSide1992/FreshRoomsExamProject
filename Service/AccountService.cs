using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Infastructure;
using Infastructure.CostumExeptions;
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

    /*
     * this method creates a new account code that can be used to create an account
     */
    public string createAccountCode()
    {
        Guid guid;
        while (true)
        {
            guid = Guid.NewGuid();
            if (!_hashRepository.checkIfGuidExist(guid.ToString()))
            {
                _hashRepository.createGuid(guid.ToString(), "user");
                return guid.ToString();
            }
        }
    }

    public User CreateUser(string userDisplayName, string userEmail, string password, string guid)
    {
        int userId = _hashRepository.getIdFromGuid(guid);
        if (userId != null && userId > 0)
        {
            var hashAlgorithm = PasswordHashAlgorithm.create();
            var salt = hashAlgorithm.generateSalt();
            var hash = hashAlgorithm.hashPassword(password, salt);

            var user = _accountRepository.createUser(userId, userDisplayName, userEmail, false);
            _hashRepository.createPasswordHash(userId, hash, salt, hashAlgorithm.getName());
            return user;
        }

        return null;
    }

    public User? Login(string email, string password)
    {
        var passwordHash = _hashRepository.getByEmail(email);
        var hashAlgorithm = PasswordHashAlgorithm.create(passwordHash.Algorithm);
        var isValid = hashAlgorithm.verifyHashedPassword(password, passwordHash.Hash, passwordHash.Salt);
        if (isValid) return _accountRepository.getById(passwordHash.id);

        return null;
    }

    public User? FindUserfromId(int id)
    {
        try
        {
            var userexists = _accountRepository.checkIfUserIsDeleted(id);
            if (userexists == 0)
            {
                return _accountRepository.getById(id);
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
            return _accountRepository.getAccountInfo(id);
        }
        catch (Exception e)
        {
            throw new AuthenticationException("The Account is deleted or does not exist");
        }
    }
    
    public string getCity(int id)
    {
        try
        {
            return _accountRepository.getCityFromUser(id);
        }
        catch (Exception e)
        {
            throw new Exception("failed to get city");
        }
    }

    
    /*
     * updates fields in database for the fields in the dto which is not N/A.
     * N/A should be set for the unchanged fields.
     * every other fields will get their values set to the new value in DB
     */
    public bool changeAccountInfo(int userInfoUserId, string? dtoNewNameDto, string? dtoNewEmailDto, string? dtoNewCityDto, string? dtoNewPasswordDto)
    {
        bool couldUpdate = false;
        if (dtoNewNameDto != null)
        {
            _accountRepository.updateName(userInfoUserId, dtoNewNameDto);
            couldUpdate = true;

        }        
        if (dtoNewEmailDto != null)
        {
            _accountRepository.updateEmail(userInfoUserId, dtoNewEmailDto);
            couldUpdate = true;

        }        
        if (dtoNewCityDto != null)
        {
            _accountRepository.updateCity(userInfoUserId, dtoNewCityDto);
            couldUpdate = true;

        }        
        if (dtoNewPasswordDto != null)
        {
            var hashAlgorithm = PasswordHashAlgorithm.create();
            var salt = hashAlgorithm.generateSalt();
            var hash = hashAlgorithm.hashPassword(dtoNewPasswordDto, salt);
            _hashRepository.updatePasswordHash(userInfoUserId, hash, salt, hashAlgorithm.getName());
            
            couldUpdate = true;
        }
        
        return couldUpdate;
    }
}