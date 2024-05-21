using Infastructure.DataModels;
using lib;

namespace api.Dtos;

public class ClientWantsToDisableOrEnableOneMotorDto : BaseDto
{
    public MotorModel motor { get; set; }
    public bool disable { get; set; }
    public int roomId { get; set; }
}