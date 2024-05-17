namespace Infastructure.DataModels;

public class BasicDeviceDModel
{
    public int roomId { get; set; }
    public double cTemp { get; set; }
    public double cHum { get; set; }
    public double cAq { get; set; }
    public bool isOpen { get; set; }
    public string deviceType { get; set; }
    
}