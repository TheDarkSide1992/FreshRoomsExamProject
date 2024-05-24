using api.Dtos;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;

namespace api.ClientRequest;

public class ClientWantsToLeaveRoom: BaseEventHandler<ClientWantsToLeaveRoomDto>
{
    public override Task Handle(ClientWantsToLeaveRoomDto dto, IWebSocketConnection socket)
    {
       WSExtentions.RemoveUserFromRoom(socket);
       return Task.CompletedTask;
    }
}