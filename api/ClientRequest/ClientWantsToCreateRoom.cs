using System.Text.Json;
using api.CostumExeptions;
using api.Dtos;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToCreateRoom(RoomService roomService) : BaseEventHandler<ClientWantsToCreateRoomDto>
{
    public override Task Handle(ClientWantsToCreateRoomDto dto, IWebSocketConnection socket)
    {
        if (roomService.CreateRoom(dto.deviceList, dto.name) != null)
        {
            var echo = new ServerRespondsToUser()
            {
                message = "You successfully create room: " + dto.name,
            };
            var messageToClient = JsonSerializer.Serialize(echo);

            socket.Send(messageToClient);
        }
        else
        {
            throw new CouldNotCreateRoomException("Could not create room, please try again");
        }
        
        
        return Task.CompletedTask;
    }
}