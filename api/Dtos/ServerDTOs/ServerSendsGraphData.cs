using lib;

namespace socketAPIFirst.Dtos;

public class ServerSendsGraphData : BaseDto
{
    public List<double> avgTempList { get; set; }
    public List<double> avgHumList { get; set; }
    public List<double> avgAqList { get; set; }
}