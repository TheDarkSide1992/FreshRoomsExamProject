using System.Text.Json;
using api.Dtos;
using api.EventFilters;
using api.State;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

[AuthenticationRequired]
public class ClientWantsToDisableOrEnableOneMotor(DeviceService _deviceService) : BaseEventHandler<ClientWantsToDisableOrEnableOneMotorDto>
{
    public override Task Handle(ClientWantsToDisableOrEnableOneMotorDto dto, IWebSocketConnection socket)
    {
        var message = "";
        if (dto.disable)
        {
            dto.motor.isDisabled = true;
            message = "The window is disabled";
        }
        else
        {
            dto.motor.isDisabled = false;
            message = "The window is enabled";
        }

        _deviceService.updateMotorstatusWithUsersInput(dto.motor);
        if ( WebSocketConnections.usersInrooms.TryGetValue(dto.roomId, out var guids))
        {
            foreach (var guid in guids)
            {
                if(WebSocketConnections.connections.TryGetValue(guid, out var ws))
                {
                    ws.Socket.Send(JsonSerializer.Serialize(new ServerReturnsNewMotorStatusForOneMotor(){motor = dto.motor, message = message}));
                }
            }
        }

        return Task.CompletedTask;
    }
}