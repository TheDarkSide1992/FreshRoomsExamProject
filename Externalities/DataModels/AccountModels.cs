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