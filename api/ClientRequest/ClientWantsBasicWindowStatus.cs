using System.Text.Json;
using api.Dtos;
using Fleck;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsBasicWindowStatus(RoomService roomService) : BaseEventHandler<ClientWantsBasicWindowStatusDto>
{
    public override Task Handle(ClientWantsBasicWindowStatusDto dto, IWebSocketConnection socket)
    {
        var mess = new ServerReturnsBasicWindowStatus()
        {
            windowStatus = roomService.getBasicRoomWindowStatus(dto.roomId),
        };
        var messageToClient = JsonSerializer.Serialize(mess);

        socket.Send(messageToClient);
        return Task.CompletedTask;
    }
}