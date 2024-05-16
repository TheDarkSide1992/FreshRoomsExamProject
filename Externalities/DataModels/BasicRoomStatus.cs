namespace Infastructure.DataModels;

public class BasicRoomStatus
{
    public int roomId { get; set; }
    public string basicWindowStatus { get; set; }
    public string basicTempSetting { get; set; }
    public string basicAqSetting { get; set; }
    public string basicHumSetting { get; set; }
    public string basicCurrentHum { get; set; }
    public string basicCurrentTemp { get; set; }
    public string basicCurrentAq { get; set; }
    
}