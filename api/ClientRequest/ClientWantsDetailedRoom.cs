using System.Text.Json;
using api.Dtos;
using api.EventFilters;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using Infastructure.DataModels;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

[AuthenticationRequired]
public class ClientWantsDetailedRoom(RoomService _roomService, DeviceService _deviceService) : BaseEventHandler<ClientWantsDetailedRoomDto>
{
    public override Task Handle(ClientWantsDetailedRoomDto dto, IWebSocketConnection socket)
    {
        socket.AddClientToRoom(dto.roomId);
        var roomName = _roomService.getRoomName(dto.roomId);
        var sensors = _deviceService.getSensorsForRoom(dto.roomId);
        var windowmotors = _deviceService.getMotorsForRoom(dto.roomId);
        socket.Send(JsonSerializer.Serialize(new ServerReturnsDetailedRoomToUser
        {
            room = new DetailedRoomModel
            {
                roomId = dto.roomId,
                name = roomName,
                sensors = sensors,
                motors = windowmotors
            }
        }));
        return Task.CompletedTask;
    }
}