namespace Infastructure.DataModels;

public class DetailedRoomModel
{
    public int roomId { get; set; }
    public string name { get; set; }
    public List<SensorModel> sensors { get; set; }
    public List<MotorModel> motors { get; set; }
}