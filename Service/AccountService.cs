using Infastructure;
using Infastructure.DataModels;
using Microsoft.Extensions.Logging;

namespace Service;

public class AccountService
{
    private readonly HashRepository _hashRepository;
    private readonly AccountRepository _accountRepository;
    private readonly ILogger<AccountService> _logger;
    
    
    public AccountService(AccountRepository accountRepository, HashRepository hashRepository,
        ILogger<AccountService> logger)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _hashRepository = hashRepository;
    }

    public string createAccountCode()
    {
        Guid guid;
        do
        {
            guid = Guid.NewGuid();
        } while (!_hashRepository.CheckIfGuidExist(guid.ToString()));

        return guid.ToString();
    }
    
    public int getIdFromGuid(string guid)
    {
        return _hashRepository.GetIdFromGuid(guid);
    }
    
    public User CreateUser(string userDisplayName, string userEmail, string password, string guid)
    {
        int userId = _hashRepository.GetIdFromGuid(guid);
        if (userId != null && userId > 0)
        {
            var hashAlgorithm = PasswordHashAlgorithm.Create();
            var salt = hashAlgorithm.GenerateSalt();
            var hash = hashAlgorithm.HashPassword(password, salt);
        
            var user = _accountRepository.CreateUser(userId, userDisplayName, userEmail);
            _hashRepository.CreatePasswordHash(userId, hash, salt, hashAlgorithm.GetName());
            return user;
        }

        return null;
    }
    
    
}