using System.Text.Json;
using api.Dtos;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsRoomList(RoomService roomService) : BaseEventHandler<ClientWantsRoomListDto>
{
    public override Task Handle(ClientWantsRoomListDto dto, IWebSocketConnection socket)
    {
        var returnList = new ServerReturnsRoomList()
        {
            roomList = roomService.GetAllRooms()
        };
        var messageToClient = JsonSerializer.Serialize(returnList);

        socket.Send(messageToClient);
        return Task.CompletedTask;
    }
}