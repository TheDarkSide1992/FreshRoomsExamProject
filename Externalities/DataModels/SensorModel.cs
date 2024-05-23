namespace Infastructure.DataModels;

public class SensorModel
{
    public string? sensorId { get; set; }
    public double temperature { get; set; }
    public double humidity { get; set; }
    public double co2 { get; set; }
    public DateTime? timestamp { get; set; }
}