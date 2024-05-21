using lib;

namespace api.Dtos;

public class ClientWantsToOpenOrCloseAllWindowsInRoomDto : BaseDto
{
    public int id { get; set; }
    public bool open { get; set; }
}