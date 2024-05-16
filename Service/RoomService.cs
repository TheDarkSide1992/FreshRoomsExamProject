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

    public RoomConfigModel getRoomPrefrencesConfiguration(int dtoRoomId)
    {
        return _roomRepository.getRoomPrefrencesConfiguration(dtoRoomId);
    }


    public RoomConfigModel updateRoomPrefrencesConfiguration(int userInfoUserId, int dtoRoomId, double dtoUpdatedMinTemperature, double dtoUpdatedMaxTemperature, double dtoUpdatedMinHumidity, double dtoUpdatedMaxHumidity, double dtoUpdatedMinAq, double dtoUpdatedMaxAq)
    {
        if (_accountRepository.isAdmin(userInfoUserId))
        {
            return _roomRepository.updateRoomPrefrencesConfiguration( dtoRoomId, dtoUpdatedMinTemperature, dtoUpdatedMaxTemperature, dtoUpdatedMinHumidity, dtoUpdatedMaxHumidity, dtoUpdatedMinAq, dtoUpdatedMaxAq);
        }
        
        throw new NotImplementedException();
    }
}