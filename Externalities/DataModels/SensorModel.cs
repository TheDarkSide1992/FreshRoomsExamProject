namespace Infastructure.DataModels;

public class SensorModel
{
    public string? sensorId { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double CO2 { get; set; }
    public DateTime? timestamp { get; set; }
}