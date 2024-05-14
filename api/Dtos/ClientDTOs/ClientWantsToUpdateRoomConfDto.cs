using lib;

namespace api.Dtos;

public class ClientWantsToUpdateRoomConfDto : BaseDto
{
    public int roomId { get; set; }
    public double updatedMinTemperature { get; set; }
    public double updatedMaxTemperature { get; set; }
    public double updatedMaxHumidity { get; set; }
    public double updatedMinHumidity { get; set; }
    public double updatedMinAq { get; set; }
    public double updatedMaxAq { get; set; }
}