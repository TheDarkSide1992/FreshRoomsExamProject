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

        if (deviceService.VerifySensorGuid(dto.sensorGuid))
        {
            socket.Send(JsonSerializer.Serialize(new ServerRespondsToSensorVeryficationDto
            {
                deviceTypeName = dto.deviceTypeName,
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