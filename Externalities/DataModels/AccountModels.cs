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

public class AccountInfo
{
    [MinLength(4)]
    public String email { get; set; }
    
    [MinLength(1)]
    public String city { get; set; }
    
    [MinLength(2)]
    public String realname { get; set; }
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