using api.Dtos;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using MqttClient = api.Mqtt.MqttClient;

namespace api.ClientRequest;

public class ClientWantsToVerifySensor(MqttClient mqttClient) : BaseEventHandler<ClientWantsToVerifySensorDto>
{
    public override Task Handle(ClientWantsToVerifySensorDto dto, IWebSocketConnection socket)
    {
        socket.AddDeviceId(dto.sensorGuid);
        mqttClient.verifyDeviceGuid(dto.sensorGuid);
        return Task.CompletedTask;
    }
}