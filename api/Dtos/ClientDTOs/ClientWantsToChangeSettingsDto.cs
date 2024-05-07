using lib;

namespace api.Dtos;

public class ClientWantsToChangeSettingsDto : BaseDto
{
    public string? newNameDto { get; set; }
    public string? newEmailDto { get; set; }
    public string? newCityDto { get; set; }
    public string? newPasswordDto { get; set; }
}