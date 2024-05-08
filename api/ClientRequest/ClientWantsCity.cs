using System.Text.Json;
using api.Dtos;
using api.EventFilters;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

[AuthenticationRequired]
[ValidateDataAnnotations]
public class ClientWantsCity(AccountService accountService): BaseEventHandler<ClientWantsCityDto>
{
    public override Task Handle(ClientWantsCityDto dto, IWebSocketConnection socket)
    {
        var metadata = socket.GetMetadata();
        var city = accountService.getCity(metadata.userInfo.userId);
        socket.Send(JsonSerializer.Serialize(new ServerReturnsCity { city = city }));
        return Task.CompletedTask;
    }
}