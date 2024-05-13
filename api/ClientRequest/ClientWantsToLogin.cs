using api.Dtos;
using api.EventFilters;
using api.StaticHelpers;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using Infastructure.DataModels;
using lib;
using Service;
using socketAPIFirst.Dtos;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace api.ClientRequest;

[rateLimiter(5)]
[ValidateDataAnnotations]
public class ClientWantsToLogin(AccountService accountService) : BaseEventHandler<ClientWantsToLoginDto>
{
    public override Task Handle(ClientWantsToLoginDto dto, IWebSocketConnection socket)
    {
        User user = accountService.Login(dto.email, dto.password);
        if (user != null)
        {
            socket.Authenticate(user);
            socket.Send(JsonSerializer.Serialize(new ServerLogsInUser { jwt = SecurityUtilities.IssueJwt(user.userId) }));
        }

        return Task.CompletedTask;
    }
}