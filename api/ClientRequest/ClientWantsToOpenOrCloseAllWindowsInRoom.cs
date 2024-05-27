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
        mqttClient.openAllWindowsWithUserInput(motors,dto.open,dto.id);
        if (dto.open)
        {
            _deviceService.updateAllMotorsInAroom(dto.id,dto.open, true);
        }
        else
        {
            _deviceService.updateAllMotorsInAroom(dto.id,dto.open, false);
        }
        return Task.CompletedTask;
    }
}