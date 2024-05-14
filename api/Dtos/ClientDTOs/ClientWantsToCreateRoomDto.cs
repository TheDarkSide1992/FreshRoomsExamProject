using Infastructure.DataModels;
using lib;

namespace api.Dtos;

public class ClientWantsToCreateRoomDto : BaseDto
{
    public string name { get; set; }
    public IEnumerable<DeviceModel> deviceList { get; set; }
}