using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsDetailedRoomToUser : BaseDto
{
    public DetailedRoomModel room { get; set; }
}