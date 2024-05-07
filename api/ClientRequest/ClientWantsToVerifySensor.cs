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
            return Task.CompletedTask;
        throw new SensorGuidNotValidException("Sensor Guid not found");
    }
}