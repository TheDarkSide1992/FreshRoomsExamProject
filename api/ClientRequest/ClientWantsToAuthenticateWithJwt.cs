using System.Text.Json;
using api.Dtos;
using api.EventFilters;
using api.Mqtt;
using api.StaticHelpers;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;
[ValidateDataAnnotations]
public class ClientWantsToAuthenticateWithJwt(AccountService accountService, MqttClient mqtt) : BaseEventHandler<ClientWantsToAuthenticateWithJwtDto>
{
    public override async Task Handle(ClientWantsToAuthenticateWithJwtDto dto, IWebSocketConnection socket)
    {
        var claims = SecurityUtilities.ValidateJwtAndReturnClaims(dto.jwt!);
        var user = accountService.FindUserfromId(int.Parse(claims["u"]));
        socket.Authenticate(user);
        await mqtt.communicateWithMqttBroker();
        socket.Send(JsonSerializer.Serialize(new ServerAuthenticatesUserFromJwt()));
        //return Task.CompletedTask;
    }
}