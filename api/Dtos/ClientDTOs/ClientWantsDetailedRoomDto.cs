using System.Security.Cryptography.X509Certificates;
using lib;

namespace api.Dtos;

public class ClientWantsDetailedRoomDto : BaseDto
{
    public int roomId { get; set; }
}