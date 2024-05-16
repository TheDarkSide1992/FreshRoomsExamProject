using System.Text.Json;
using api.Dtos;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsBasicRoomStatus(RoomService roomService) : BaseEventHandler<ClientWantsBasicRoomStatusDto>
{
    public override Task Handle(ClientWantsBasicRoomStatusDto dto, IWebSocketConnection socket)
    {
        var mess = new ServerReturnsBasicRoomStatus()
        {
            basicRoomListData = roomService.getBasicRoomWindowStatus(),
        };
        var messageToClient = JsonSerializer.Serialize(mess);

        socket.Send(messageToClient);
        return Task.CompletedTask;
    }
}