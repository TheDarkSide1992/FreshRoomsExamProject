using System.Text.Json;
using api.CostumExeptions;
using api.Dtos;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToVerifySensor(DeviceService deviceService) : BaseEventHandler<ClientWantsToVerifySensorDto>
{
    public override Task Handle(ClientWantsToVerifySensorDto dto, IWebSocketConnection socket)
    {
        string deviceType = deviceService.VerifySensorGuid(dto.sensorGuid);

        if (deviceType != null && deviceType != "")
        {
            socket.Send(JsonSerializer.Serialize(new ServerRespondsToSensorVeryficationDto
            {
                deviceTypeName = deviceType,
                foundSensor = true,
                sensorGuid = dto.sensorGuid
            }));
        }
        else
        {
            throw new SensorGuidNotValidException("Sensor Guid not found");
        }
        return Task.CompletedTask;
    }
}