using api.Dtos;
using api.EventFilters;
using api.Mqtt;
using Fleck;
using lib;
using Service;

namespace api.ClientRequest;

[AuthenticationRequired]
public class ClientWantsToOpenOrCloseAllWindowsInRoom(DeviceService _deviceService, MqttClient mqttClient) : BaseEventHandler<ClientWantsToOpenOrCloseAllWindowsInRoomDto>
{
    public override Task Handle(ClientWantsToOpenOrCloseAllWindowsInRoomDto dto, IWebSocketConnection socket)
    {
        var motors = _deviceService.getMotorsForRoom(dto.id);
        var newMotors = mqttClient.OpenAllWindowsWithUserInput(motors,dto.open,dto.id);
        foreach (var motor in newMotors)
        {
            _deviceService.updateMoterstatus(motor);
        }
        return Task.CompletedTask;
    }
}