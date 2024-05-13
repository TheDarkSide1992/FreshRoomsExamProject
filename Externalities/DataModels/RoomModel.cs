namespace Infastructure.DataModels;

public class RoomModel
{
    public int roomId { get; set; }
    public string name { get; set; }
    public int creatorId { get; set; }
    public IEnumerable<DeviceModel> deviceList { get; set; }
}