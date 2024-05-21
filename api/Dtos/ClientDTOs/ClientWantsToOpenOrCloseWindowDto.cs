using Infastructure.DataModels;
using lib;

namespace api.Dtos;

public class ClientWantsToOpenOrCloseWindowDto : BaseDto
{
    public int roomId { get; set; }
    public MotorModel motor { get; set; } 
    public bool open { get; set; }
}