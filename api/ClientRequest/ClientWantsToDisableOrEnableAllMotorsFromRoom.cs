using System.Text.Json;
using api.Dtos;
using api.State;
using Fleck;
using Infastructure.DataModels;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToDisableOrEnableAllMotorsFromRoom(DeviceService _deviceService) : BaseEventHandler<ClientWantsToDisableOrEnableAllMotorsFromRoomDto>
{
    public override Task Handle(ClientWantsToDisableOrEnableAllMotorsFromRoomDto dto, IWebSocketConnection socket)
    {
        var message = "";
        var motors = new List<MotorModel>();
            if (dto.disable)
            {
                motors = _deviceService.updateAllMotorsInAroom(dto.roomId, false, dto.disable);
                message = "All windows are disabled";
            }
            else
            {
                motors = _deviceService.updateAllMotorsInAroom(dto.roomId, false, dto.disable);
                message = "All windows are enabled";
            }
        if ( WebSocketConnections.usersInrooms.TryGetValue(dto.roomId, out var guids))
        {
            foreach (var guid in guids)
            {
                if(WebSocketConnections.connections.TryGetValue(guid, out var ws))
                {
                    ws.Socket.Send(JsonSerializer.Serialize(new ServerReturnsNewMotorStatusForAllMotorsInRoom(){motors = motors, message = message}));
                }
            }
        }

        return Task.CompletedTask;
    }
}