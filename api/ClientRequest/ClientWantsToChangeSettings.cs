using System.Text.Json;
using api.Dtos;
using api.StaticHelpers;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToChangeSettings(AccountService accountService) : BaseEventHandler<ClientWantsToChangeSettingsDto>
{
    public override Task Handle(ClientWantsToChangeSettingsDto dto, IWebSocketConnection socket)
    {
        
        var metData = socket.GetMetadata();
        bool succes = accountService.changeAccountInfo(metData.userInfo.userId, dto.newNameDto, dto.newEmailDto, dto.newCityDto, dto.newPasswordDto);


        if (succes)
        {
            var echo = new ServerRespondsToUser()
            {
                message = "You succesfully Udated your changes | Status : " +succes,
            };
            var messageToClient = JsonSerializer.Serialize(echo);

            socket.Send(messageToClient);
        }
        else
        {
            var echo = new ServerRespondsToUser()
            {
                message = "could not update changes | Status : " +succes,
            };
            var messageToClient = JsonSerializer.Serialize(echo);

            socket.Send(messageToClient);
        }
        return Task.CompletedTask;
    }
}