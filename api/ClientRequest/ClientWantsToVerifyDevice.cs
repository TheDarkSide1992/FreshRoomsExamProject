using api.Dtos;
using api.EventFilters;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using MqttClient = api.Mqtt.MqttClient;

namespace api.ClientRequest;
[AuthenticationRequired]
public class ClientWantsToVerifyDevice(MqttClient mqttClient) : BaseEventHandler<ClientWantsToVerifyDeviceDto>
{
    public override Task Handle(ClientWantsToVerifyDeviceDto dto, IWebSocketConnection socket)
    {
        socket.addDeviceId(dto.sensorGuid);
        mqttClient.verifyDeviceGuid(dto.sensorGuid);
        return Task.CompletedTask;
    }
}