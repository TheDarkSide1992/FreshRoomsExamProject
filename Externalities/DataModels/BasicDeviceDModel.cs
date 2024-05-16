namespace Infastructure.DataModels;

public class BasicDeviceDModel
{
    public int roomId { get; set; }
    public double avgTemp { get; set; }
    public double avgHum { get; set; }
    public double avgAq { get; set; }
    public bool isOpen { get; set; }
    public string deviceType { get; set; }
    
}