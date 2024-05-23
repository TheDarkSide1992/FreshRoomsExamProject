using Infastructure.DataModels;
using lib;

namespace api.Dtos;

public class ClientWantsToDisableOrEnableAllMotorsFromRoomDto : BaseDto
{
    public int roomId { get; set; }
    public bool disable { get; set; }
}