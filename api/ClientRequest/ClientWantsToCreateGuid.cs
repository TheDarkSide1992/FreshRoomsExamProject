using System.Text.Json;
using api.Dtos;
using api.EventFilters;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

[AuthenticationRequired]
[ValidateDataAnnotations]
public class ClientWantsToCreateGuid(AccountService accountService) : BaseEventHandler<ClientWantsToCreateGuidDto>
{
    public override Task Handle(ClientWantsToCreateGuidDto dto, IWebSocketConnection socket)
    {
    string guid = accountService.createAccountCode();
        var echo = new ServerRespondsToUser()
        {
            message = "guid: " + guid,
        };
        var messageToClient = JsonSerializer.Serialize(echo);

        socket.Send(messageToClient);
        
        return Task.CompletedTask;
    }
}