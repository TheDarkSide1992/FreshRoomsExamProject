using System.Text.Json;
using api.Dtos;
using api.StaticHelpers;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToAuthenticateWithJwt(AccountService accountService) : BaseEventHandler<ClientWantsToAuthenticateWithJwtDto>
{
    public override Task Handle(ClientWantsToAuthenticateWithJwtDto dto, IWebSocketConnection socket)
    {
        var claims = SecurityUtilities.ValidateJwtAndReturnClaims(dto.jwt!);
        var user = accountService.FindUserfromId(int.Parse(claims["userId"]));
        socket.Authenticate(user);
        socket.Send(JsonSerializer.Serialize(new ServerAuthenticatesUserFromJwt()));
        return Task.CompletedTask;
    }
}