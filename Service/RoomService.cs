using Infastructure;
using Infastructure.DataModels;

namespace Service;

public class RoomService
{
    private readonly RoomRepository _roomRepository;
    private readonly DeviceRepository _deviceRepository;
    private readonly AccountRepository _accountRepository;

    public RoomService(RoomRepository roomRepository, DeviceRepository deviceRepository,
        AccountRepository accountRepository)
    {
        _roomRepository = roomRepository;
        _deviceRepository = deviceRepository;
        _accountRepository = accountRepository;
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

    public RoomConfigModel getRoomPrefrencesConfiguration(int userInfoUserId, int dtoRoomId)
    {
        return _roomRepository.getRoomPrefrencesConfiguration(userInfoUserId, dtoRoomId);
    }
}