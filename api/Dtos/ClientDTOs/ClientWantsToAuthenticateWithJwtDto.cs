using System.ComponentModel.DataAnnotations;
using lib;

namespace api.Dtos;

public class ClientWantsToAuthenticateWithJwtDto : BaseDto
{
    [Required]
    public string? jwt { get; set; }
}