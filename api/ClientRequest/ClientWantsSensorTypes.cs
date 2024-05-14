using System.Collections;
using System.Text.Json;
using api.CostumExeptions;
using api.Dtos;
using Fleck;
using Infastructure.DataModels;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsSensorTypes(DeviceService deviceService) : BaseEventHandler<ClientWantsSensorTypesDto>
{
    public override Task Handle(ClientWantsSensorTypesDto dto, IWebSocketConnection socket)
    {
        try
        {
            IEnumerable<DeviceTypeModel> arrayList = deviceService.getSensorTypes();
            socket.Send(JsonSerializer.Serialize(new ServerSendsDeviceTypes()
            {
                deviceTypeList = arrayList
            }));
        }
        catch (Exception e)
        {
            throw new DeviceTypesNotFoundException("Device Types could not be found");
        }

        return Task.CompletedTask;
    }
}