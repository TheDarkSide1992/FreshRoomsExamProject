using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsBasicWindowStatus : BaseDto
{
    public string windowStatus { get; set; }
    public int roomId { get; set; }
}