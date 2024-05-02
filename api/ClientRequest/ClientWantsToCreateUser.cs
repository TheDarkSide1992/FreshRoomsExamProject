using System.Text.Json;
using api.Dtos;
using Fleck;
using Infastructure.DataModels;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToCreateUser(AccountService accountService) : BaseEventHandler<ClientWantsToCreateUserDto>
{
    public override Task Handle(ClientWantsToCreateUserDto dto, IWebSocketConnection socket)
    {
        User user = accountService.CreateUser(dto.name, dto.email, dto.password, dto.guid);
        if (user != null)
        {
            var echo = new ServerRespondsToUser()
            {
                message = "You succesfully registered on freshrooms: " + user.userDisplayName,
            };
            var messageToClient = JsonSerializer.Serialize(echo);

            socket.Send(messageToClient);
        }
       
        
        return Task.CompletedTask;
    }
}