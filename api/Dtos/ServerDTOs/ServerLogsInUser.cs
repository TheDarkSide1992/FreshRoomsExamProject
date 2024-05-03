using lib;

namespace socketAPIFirst.Dtos;

public class ServerLogsInUser : BaseDto
{
    public string jwt { get; set; }
}