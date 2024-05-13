using System.Text.Json;
using api.Dtos;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsRoomConfigurations() : BaseEventHandler<ClientWantsRoomConfigurationsDto>
{
    public override Task Handle(ClientWantsRoomConfigurationsDto dto, IWebSocketConnection socket)
    {
        //var metData = socket.GetMetadata();
        var roomConfig = new ServerSendsRoomConfigurations(){
            minTemparature = 12.0,
            maxTemparature = 22.0,
            maxHumidity = 25.5,
            minHumidity = 2.0,
            minAq = 1.0,
            maxAq = 2.0,
        };

        Console.WriteLine("Made Mock");
        
        var messageToClient = JsonSerializer.Serialize(roomConfig);
        socket.Send(messageToClient);
        
        return Task.CompletedTask;
    }
}