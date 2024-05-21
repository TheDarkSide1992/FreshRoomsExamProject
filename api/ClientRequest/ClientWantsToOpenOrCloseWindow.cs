using System.Text.Json;
using api.Dtos;
using api.Mqtt;
using api.State;
using Fleck;
using Infastructure.DataModels;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToOpenOrCloseWindow(DeviceService _deviceService, MqttClient mqttClient) : BaseEventHandler<ClientWantsToOpenOrCloseWindowDto>
{
    public override Task Handle(ClientWantsToOpenOrCloseWindowDto dto, IWebSocketConnection socket)
    {
        var message = "";
        mqttClient.openOrCloseAWindow(dto.motor, dto.open);
        if (dto.open)
        {
            message = "Window is open or being opened";
            dto.motor.isOpen = true;
            dto.motor.isDisabled = true;
        }
        else
        {
            message = "Window is closed or being closed";
            dto.motor.isOpen = false;
            dto.motor.isDisabled = false;
        }
        _deviceService.updateMoterstatus(dto.motor);
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