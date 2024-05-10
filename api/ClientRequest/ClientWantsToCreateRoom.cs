using api.Dtos;
using Fleck;
using lib;
using Service;

namespace api.ClientRequest;

public class ClientWantsToCreateRoom(RoomService roomService) : BaseEventHandler<ClientWantsToCreateRoomDto>
{
    public override Task Handle(ClientWantsToCreateRoomDto dto, IWebSocketConnection socket)
    {
        roomService.CreateRoom(dto.deviceList, dto.name);
        Console.WriteLine("testing of createRoom works");
        return Task.CompletedTask;
    }
}