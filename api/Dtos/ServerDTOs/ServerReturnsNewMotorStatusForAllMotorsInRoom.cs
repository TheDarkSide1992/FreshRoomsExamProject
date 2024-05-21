using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsNewMotorStatusForAllMotorsInRoom: BaseDto
{
    public List<MotorModel> motors { get; set; }
    public string message { get; set; }
}