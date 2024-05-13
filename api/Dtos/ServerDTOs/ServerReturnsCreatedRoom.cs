using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsCreatedRoom : BaseDto
{
    public int roomId { get; set; }
    public string name { get; set; }
    public int creatorId { get; set; }
    public IEnumerable<DeviceModel> deviceList { get; set; }
}