using lib;

namespace socketAPIFirst.Dtos;

public class ServerRespondsToSensorVeryficationDto : BaseDto
{
    public bool foundSensor { get; set; }
}