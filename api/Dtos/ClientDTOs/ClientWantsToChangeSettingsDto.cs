using System.ComponentModel.DataAnnotations;
using lib;

namespace api.Dtos;

public class ClientWantsToChangeSettingsDto : BaseDto
{
    public string? newNameDto { get; set; }
    [EmailAddress]
    public string? newEmailDto { get; set; }
    public string? newCityDto { get; set; }
    [MinLength(8)]
    [MaxLength(32)]
    public string? newPasswordDto { get; set; }
}