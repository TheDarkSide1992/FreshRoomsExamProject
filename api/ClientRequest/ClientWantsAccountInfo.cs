using api.Dtos;
using api.StaticHelpers;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using System.Text.Json;
using api.EventFilters;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

[AuthenticationRequired]
[ValidateDataAnnotations]
public class ClientWantsAccountInfo(AccountService accountService) : BaseEventHandler<ClientWantsAccountInfoDto>
{
    public override Task Handle(ClientWantsAccountInfoDto dto, IWebSocketConnection socket)
    {
        
        var metData = socket.getMetadata();
        var accountInfo = accountService.getAccountnfo(metData.userInfo.userId);

        if (accountInfo != null)
        {
            var serverSendsAccountData = new ServerSendsAccountData()
            {
                realname = accountInfo.realname,
                city = accountInfo.city,
                email = accountInfo.email,
            };

            var messageToClient = JsonSerializer.Serialize(serverSendsAccountData);
            socket.Send(messageToClient);
        }
        else
        {
            var serverSendsAccountData = new ServerSendsAccountData()
            {
                realname = "N/A",
                city = "N/A",
                email = "N/A",
            };
            var messageToClient = JsonSerializer.Serialize(serverSendsAccountData);
            socket.Send(messageToClient);
        }

        return Task.CompletedTask;
    }
    
}