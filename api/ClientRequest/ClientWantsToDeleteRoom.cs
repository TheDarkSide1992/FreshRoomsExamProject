using System.Text.Json;
using api.CostumExeptions;
using api.Dtos;
using api.EventFilters;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

[AuthenticationRequired]
public class ClientWantsToDeleteRoom(RoomService roomService) : BaseEventHandler<ClientWantsToDeleteRoomDto>
{
    public override Task Handle(ClientWantsToDeleteRoomDto dto, IWebSocketConnection socket)
    {
        if (roomService.DeleteRoom(dto.roomId))
        {
            var echo = new ServerRespondsToUser()
            {
                message = "You successfully Deleted the Room: " + dto.roomName,
            };
            var messageToClient = JsonSerializer.Serialize(echo); ;
            socket.Send(messageToClient);
            return Task.CompletedTask;
        }
        else
        {
            throw new CouldNotDeleteRoomException("Could not delete room, please try again");
        }
    }
}