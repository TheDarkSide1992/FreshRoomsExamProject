using lib;

namespace api.Dtos;

public class ClientWantsToVerifySensorDto : BaseDto
{
    public string deviceTypeName { get; set; }
    public string sensorGuid { get; set; }
}