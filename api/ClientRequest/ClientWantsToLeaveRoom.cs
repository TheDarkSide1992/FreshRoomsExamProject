using api.Dtos;
using api.EventFilters;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;

namespace api.ClientRequest;

[AuthenticationRequired]
public class ClientWantsToLeaveRoom: BaseEventHandler<ClientWantsToLeaveRoomDto>
{
    public override Task Handle(ClientWantsToLeaveRoomDto dto, IWebSocketConnection socket)
    {
       WSExtentions.removeUserFromRoom(socket);
       return Task.CompletedTask;
    }
}