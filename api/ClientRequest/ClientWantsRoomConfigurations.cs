using System.Text.Json;
using api.Dtos;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsRoomConfigurations(RoomService service) : BaseEventHandler<ClientWantsRoomConfigurationsDto>
{
    public override Task Handle(ClientWantsRoomConfigurationsDto dto, IWebSocketConnection socket)
    {
        var metData = socket.GetMetadata();

        var roomConf = service.getRoomPrefrencesConfiguration(metData.userInfo.userId, dto.roomId);
        var roomConfig = new ServerSendsRoomConfigurations(){
            minTemparature = roomConf.minTemparature,
            maxTemparature = roomConf.maxTemparature,
            maxHumidity = roomConf.maxHumidity,
            minHumidity = roomConf.minHumidity,
            minAq = roomConf.minAq,
            maxAq = roomConf.maxAq,
        };

        var messageToClient = JsonSerializer.Serialize(roomConfig);
        socket.Send(messageToClient);
        
        return Task.CompletedTask;
    }
}