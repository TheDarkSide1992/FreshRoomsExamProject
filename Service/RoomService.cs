using Infastructure;
using Infastructure.DataModels;

namespace Service;

public class RoomService
{
    private readonly RoomRepository _roomRepository;
    private readonly DeviceRepository _deviceRepository;

    public RoomService(RoomRepository roomRepository, DeviceRepository deviceRepository)
    {
        _roomRepository = roomRepository;
        _deviceRepository = deviceRepository;
    }

    public RoomModel CreateRoom(IEnumerable<DeviceModel> deviceList, string name)
    {
        RoomModel roomModel = _roomRepository.CreateRoom(name);
        if (roomModel != null)
        { 
            _deviceRepository.UpdateDevices(deviceList, roomModel.roomId);
            return roomModel;
        }
        return null;
        

        

    }
}