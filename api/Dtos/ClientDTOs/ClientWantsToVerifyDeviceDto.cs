using lib;

namespace api.Dtos;

public class ClientWantsToVerifyDeviceDto : BaseDto
{
    public string deviceTypeName { get; set; }
    public string sensorGuid { get; set; }
}