using lib;

namespace api.Dtos;

public class ClientWantsToCreateUserDto : BaseDto
{
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string guid { get; set; }
}