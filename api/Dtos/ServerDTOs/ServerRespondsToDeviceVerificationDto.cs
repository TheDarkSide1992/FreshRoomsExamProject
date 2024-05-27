using lib;

namespace socketAPIFirst.Dtos;

public class ServerRespondsToDeviceVerificationDto : BaseDto
{
    public bool foundDevice { get; set; }
    public string deviceTypeName { get; set; }
    public string deviceGuid { get; set; }
}