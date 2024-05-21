using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsNewMotorStatusForOneMotor : BaseDto
{
    public MotorModel motor { get; set; }
    public string message { get; set; }
}