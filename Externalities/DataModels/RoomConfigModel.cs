namespace Infastructure.DataModels;

public class RoomConfigModel
{
    public double minTemparature { get; set; }
    public double maxTemparature { get; set; }
    public double maxHumidity { get; set; }
    public double minHumidity { get; set; }
    public double minAq { get; set; }
    public double maxAq { get; set; }
}