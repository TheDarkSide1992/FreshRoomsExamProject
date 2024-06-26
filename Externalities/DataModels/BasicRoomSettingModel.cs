namespace Infastructure.DataModels;

public class BasicRoomSettingModel
{
    public int roomId { get; set; }
    public string roomName { get; set; }
    public double minTemparature { get; set; }
    public double maxTemparature { get; set; }
    public double maxHumidity { get; set; }
    public double minHumidity { get; set; }
    public double minAq { get; set; }
    public double maxAq { get; set; }
}