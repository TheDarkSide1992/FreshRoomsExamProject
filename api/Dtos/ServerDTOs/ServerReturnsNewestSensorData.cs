using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsNewestSensorData : BaseDto
{
    public SensorModel data { get; set; }
}