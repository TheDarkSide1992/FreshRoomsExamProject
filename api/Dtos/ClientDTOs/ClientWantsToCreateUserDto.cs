using System.ComponentModel.DataAnnotations;
using lib;

namespace api.Dtos;

public class ClientWantsToCreateUserDto : BaseDto
{
    public string name { get; set; }
    [EmailAddress]
    public string email { get; set; }
    [MinLength(8)]
    [MaxLength(32)]
    public string password { get; set; }
    public string guid { get; set; }
}