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

    public RoomModel CreateRoom(IEnumerable<DeviceModel> deviceList, string name, int createdBy)
    {
        RoomModel roomModel = _roomRepository.CreateRoom(name, createdBy);
        if (roomModel != null)
        {
            _roomRepository.CreateRoomConfig(roomModel.roomId);
            _deviceRepository.UpdateDevices(deviceList, roomModel.roomId);
            return roomModel;
        }
        return null;
    }

    public bool DeleteRoom(int roomId)
    {
        if (_roomRepository.DeleteRoomConfig(roomId) && _deviceRepository.DeleteRoomIdOnDevices(roomId))
        {
            return _roomRepository.DeleteRoom(roomId);
        }
        else
        {
            throw new Exception("Could not delete room");
        }
    }
    
    public IEnumerable<RoomModel> GetAllRooms()
    {
        return _roomRepository.GetAllRooms();
    }
    
    public RoomConfigModel getRoomPrefrencesConfiguration(int userInfoUserId, int dtoRoomId)
    {
        return _roomRepository.getRoomPrefrencesConfiguration(userInfoUserId, dtoRoomId);
    }


    public RoomConfigModel updateRoomPrefrencesConfiguration(int userInfoUserId, int dtoRoomId, double dtoUpdatedMinTemperature, double dtoUpdatedMaxTemperature, double dtoUpdatedMinHumidity, double dtoUpdatedMaxHumidity, double dtoUpdatedMinAq, double dtoUpdatedMaxAq)
    {
        if (_accountRepository.isAdmin(userInfoUserId))
        {
            return _roomRepository.updateRoomPrefrencesConfiguration( dtoRoomId, dtoUpdatedMinTemperature, dtoUpdatedMaxTemperature, dtoUpdatedMinHumidity, dtoUpdatedMaxHumidity, dtoUpdatedMinAq, dtoUpdatedMaxAq);
        }
        
        throw new NotImplementedException();
    }

    public string getBasicRoomWindowStatus()
    {
        IEnumerable<BasicRoomStatus> roomStatusList = _deviceRepository.getMotorsForRoom();
        IEnumerable<RoomConfigModel> roomConfigModels = _roomRepository.GetALLRoomSettings();
        
        foreach (var roomConfig in roomConfigModels)
        {
            
        }
        /*string output = "";
        var motorList = _deviceRepository.getMotorsForRoom();
        foreach (var motor in motorList)
        {
            if (motor.isOpen)
            {
                output = "Open";
            }
            else if(!motor.isOpen && output != "")
            {
                return "Mixed";
            }
            else if(!motor.isOpen)
            {
                output = "Closed";
            }
        }
        return output;*/
    }
}