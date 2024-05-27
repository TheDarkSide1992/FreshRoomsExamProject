using System.Security.Authentication;
using api.Dtos;
using api.EventFilters;
using api.Mqtt;
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
public class ClientWantsToLogin(AccountService accountService, MqttClient mqttClient) : BaseEventHandler<ClientWantsToLoginDto>
{
    public override  Task Handle(ClientWantsToLoginDto dto, IWebSocketConnection socket)
    {
        try
        {
            User user = accountService.Login(dto.email, dto.password);
            if (user != null)
                {
                    socket.authenticate(user);
                    if (mqttClient._client == null)
                    {
                        mqttClient.communicateWithMqttBroker();
                    }
                    socket.Send(JsonSerializer.Serialize(new ServerLogsInUser { jwt = SecurityUtilities.issueJwt(user.userId) }));
                }
        }
        catch (Exception e)
        {
            throw new AuthenticationException("Wrong password or email, please try again");
        }
        return Task.CompletedTask;
    }
}