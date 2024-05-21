using System.Text.Json;
using api.Dtos;
using api.EventFilters;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

[AuthenticationRequired]
public class ClientWantsToUpdateRoomConf(RoomService service) : BaseEventHandler<ClientWantsToUpdateRoomConfDto>
{
    public override Task Handle(ClientWantsToUpdateRoomConfDto dto, IWebSocketConnection socket)
    {
        var metData = socket.GetMetadata();

        var roomConf = service.updateRoomPrefrencesConfiguration(metData.userInfo.userId, dto.roomId, dto.updatedMinTemperature, dto.updatedMaxTemperature, dto.updatedMinHumidity, dto.updatedMaxHumidity, dto.updatedMinAq, dto.updatedMaxAq);
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