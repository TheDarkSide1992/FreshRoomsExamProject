using lib;

namespace socketAPIFirst.Dtos;

public class ServerRespondsToDeviceVerificationDto : BaseDto
{
    public bool foundSensor { get; set; }
    public string deviceTypeName { get; set; }
    public string sensorGuid { get; set; }
}