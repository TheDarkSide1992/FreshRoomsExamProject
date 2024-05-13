using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsRoomList : BaseDto
{
    public IEnumerable<RoomModel> roomList { get; set; }
}