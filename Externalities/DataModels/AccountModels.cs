using System.ComponentModel.DataAnnotations;

namespace Infastructure.DataModels;

public class User
{
    public int userId { get; set; }
    
    [MaxLength(100)]
    [MinLength(1)]
    public string userDisplayName { get; set; }
    
    [EmailAddress]
    public string userEmail { get; set; }
}

public class AccountCode
{
    public int accountId { get; set; }
    
    [MinLength(1)]
    public string accountCode { get; set; }
    
    [MinLength(1)]
    public string accountCodePerms { get; set; }
    
    public bool isUsed { get; set; }
    
}