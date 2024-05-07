using lib;

namespace api.Dtos;

public class ClientWantsToVerifySensorDto : BaseDto
{
    public string name { get; set; }
    public string sensorGuid { get; set; }
}