namespace Infastructure.DataModels;

public class BasicRoomStatus
{
    public int roomId { get; set; }
    public string roomName { get; set; }
    public string basicWindowStatus { get; set; }
    public string basicTempSetting { get; set; }
    public string basicAqSetting { get; set; }
    public string basicHumSetting { get; set; }
    public double basicCurrentHum { get; set; }
    public double basicCurrentTemp { get; set; }
    public double basicCurrentAq { get; set; }
    
}