using System.Text.Json;
using api.CostumExeptions;
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
public class ClientWantsToCreateRoom(RoomService roomService) : BaseEventHandler<ClientWantsToCreateRoomDto>
{
    public override Task Handle(ClientWantsToCreateRoomDto dto, IWebSocketConnection socket)
    {
        RoomModel room = roomService.CreateRoom(dto.deviceList, dto.name, socket.getMetadata().userInfo.userId);
        if (room != null)
        {
            
            //TODO make this return one room to add to frontend list instead of override
            var mess = new ServerReturnsBasicRoomStatus()
            {
                basicRoomListData = roomService.getBasicRoomWindowStatus(),
            };
            
            var echo = new ServerRespondsToUser()
            {
                message = "You successfully create room: " + dto.name,
            };
            var roomToClient = JsonSerializer.Serialize(mess);
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