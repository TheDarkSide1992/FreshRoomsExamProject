using System.Text.Json;
using api.Dtos;
using api.Models;
using Fleck;
using lib;
using Microsoft.VisualBasic;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsGraphData(RoomService service) : BaseEventHandler<ClientWantsGraphDataDto>
{
    public override Task Handle(ClientWantsGraphDataDto dto, IWebSocketConnection socket)
    {
        var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH");
        var currentHour = DateTime.Parse(currentTime + ":00:00.000000");
        List<double> tempList = new List<double>();
        List<double> humList = new List<double>();
        List<double> aqList = new List<double>();
        GraphModel graphModel = new GraphModel();
        graphModel.startInterval.Equals(currentHour);
        graphModel.endInterval.Equals(currentHour.AddHours(1));
        for (int i = 0; i <= 23; i++)
        {
            graphModel.startInterval.Hour.Equals(currentHour.AddHours(-i));
            if (graphModel.startInterval.Hour.Equals(-1))
            {
                graphModel.startInterval.Hour.Equals(23);
                graphModel.endInterval.Hour.Equals(0);
                graphModel.startInterval.Date.Equals(currentHour.AddDays(-1));
            }
            else if(int.Parse(graphModel.startInterval.Hour.Equals(currentHour.AddHours(-i)).ToString()) <= -2)
            {
                graphModel.startInterval.Hour.Equals(currentHour.AddHours(-(i+24)));
                graphModel.endInterval.Hour.Equals(-(i+25));
                graphModel.startInterval.Date.Equals(currentHour.AddDays(-1));
                graphModel.endInterval.Date.Equals(currentHour.AddDays(-1));
            }
            else
            {
                graphModel.endInterval.Hour.Equals(currentHour.AddHours(-(i+1)));
            }
            var graphData = service.GetGraphData(dto.roomId, graphModel.startInterval, graphModel.endInterval);
            tempList.Add(graphData.avgTemperature);
            humList.Add(graphData.avgHumidity);
            aqList.Add(graphData.avgCO2);
        }
        var graphDataIntoLists = new ServerSendsGraphData()
        {
            avgHumList = humList,
            avgTempList = tempList,
            avgAqList = aqList,
        };
        
        var messageToClient = JsonSerializer.Serialize(graphDataIntoLists);
        socket.Send(messageToClient);
        
        return Task.CompletedTask;
    }
}