using System.ComponentModel.DataAnnotations;
using lib;

namespace api.Dtos;

public class ClientWantsToLoginDto : BaseDto
{
    [EmailAddress]
    public string email { get; set; }
    public string password { get; set; }
}