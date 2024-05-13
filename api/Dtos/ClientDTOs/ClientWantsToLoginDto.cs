using System.ComponentModel.DataAnnotations;
using lib;

namespace api.Dtos;

public class ClientWantsToLoginDto : BaseDto
{
    [EmailAddress]
    public string email { get; set; }
    [MinLength(8)]
    [MaxLength(32)]
    public string password { get; set; }
}