using System.Text.Json;
using api.Dtos;
using api.State;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToDisableOrEnableAllMotorsFromRoom(DeviceService _deviceService) : BaseEventHandler<ClientWantsToDisableOrEnableAllMotorsFromRoomDto>
{
    public override Task Handle(ClientWantsToDisableOrEnableAllMotorsFromRoomDto dto, IWebSocketConnection socket)
    {
        var message = "";
        var motors = _deviceService.getMotorsForRoom(dto.roomId);
        foreach (var motor in motors)
        {
            if (dto.disable)
            {
                motor.isDisabled = true;
                message = "All windows are disabled";
            }
            else
            {
                motor.isDisabled = false;
                message = "All windows are enabled";
            }
            _deviceService.updateMoterstatus(motor);
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