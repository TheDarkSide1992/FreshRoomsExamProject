using lib;

namespace api.Dtos;

public class ClientWantsToDeleteRoomDto : BaseDto
{
    public int roomId { get; set; }
    public string roomName { get; set; }
}