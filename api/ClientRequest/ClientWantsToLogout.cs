using System.Text.Json;
using api.Dtos;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToLogout: BaseEventHandler<ClientWantsToLogoutDto>
{
    public override Task Handle(ClientWantsToLogoutDto dto, IWebSocketConnection socket)
    {
        socket.UnAuthenticate();
        socket.Send(JsonSerializer.Serialize(new ServerLogsoutUser()));
        return Task.CompletedTask;
    }
}