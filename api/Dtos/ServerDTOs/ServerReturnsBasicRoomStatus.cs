using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsBasicRoomStatus : BaseDto
{
    public IEnumerable<BasicRoomStatus> basicRoomListData { get; set; }
}