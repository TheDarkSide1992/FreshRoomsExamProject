using System.Text.Json;
using api.CostumExeptions;
using api.Dtos;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using Infastructure.DataModels;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

public class ClientWantsToCreateRoom(RoomService roomService) : BaseEventHandler<ClientWantsToCreateRoomDto>
{
    public override Task Handle(ClientWantsToCreateRoomDto dto, IWebSocketConnection socket)
    {
        RoomModel room = roomService.CreateRoom(dto.deviceList, dto.name, socket.GetMetadata().userInfo.userId);
        if (room != null)
        {
            var tempRoom = new ServerReturnsCreatedRoom()
            {
                roomId = room.roomId,
                name = room.name,
                creatorId = room.creatorId,
            };
            
            
            var echo = new ServerRespondsToUser()
            {
                message = "You successfully create room: " + dto.name,
            };
            var roomToClient = JsonSerializer.Serialize(tempRoom);
            var messageToClient = JsonSerializer.Serialize(echo);
            socket.Send(roomToClient);
            socket.Send(messageToClient);
        }
        else
        {
            throw new CouldNotCreateRoomException("Could not create room, please try again");
        }
        
        
        return Task.CompletedTask;
    }
}