using System.Text.Json;
using api.Dtos;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToUpdateRoomConf(RoomService service) : BaseEventHandler<ClientWantsToUpdateRoomConfDto>
{
    public override Task Handle(ClientWantsToUpdateRoomConfDto dto, IWebSocketConnection socket)
    {
        Console.WriteLine("Updateing");
        
        var metData = socket.GetMetadata();

        var roomConf = service.updateRoomPrefrencesConfiguration(metData.userInfo.userId, dto.roomId);
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

    }
}