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

    public IEnumerable<BasicRoomStatus> getBasicRoomWindowStatus()
    {
        List<BasicRoomStatus> roomStatusList = new List<BasicRoomStatus>();
        IEnumerable<BasicRoomSettingModel> roomConfigModels = _roomRepository.GetALLRoomSettings();
        List<BasicDeviceDModel> basicDeviceDList = _deviceRepository.GetBasicDeviceData();
        
        foreach (var roomConfig in roomConfigModels)
        {
            Console.WriteLine("Roomconfig here: " + roomConfig.roomName + ", " + roomConfig.roomId);
            BasicRoomStatus roomStatus = new BasicRoomStatus();
            int countD = 0;
            string output = "";
            BasicDeviceDModel dModel = null;
            foreach (var dInfo in basicDeviceDList)
            {
                
                dModel = dInfo;
                if (roomConfig.roomId == dInfo.roomId)
                {
                    if (roomStatus.roomId == 0 || roomStatus.roomId == null)
                    {
                        roomStatus.roomId = roomConfig.roomId;
                        roomStatus.roomName = roomConfig.roomName;
                        roomStatus.basicAqSetting = roomConfig.minAq + " - " + roomConfig.maxAq;
                        roomStatus.basicHumSetting = roomConfig.minHumidity + " - " + roomConfig.maxHumidity;
                        roomStatus.basicTempSetting = roomConfig.minTemparature + " - " + roomConfig.maxTemparature;
                    }

                    if (dInfo.deviceType == "Window Motor")
                    {
                        if (dInfo.isOpen)
                        {
                            output = "Open";
                        }
                        else if(!dInfo.isOpen && output != "")
                        {
                            roomStatus.basicWindowStatus = "Mixed";
                        }
                        else if(!dInfo.isOpen)
                        {
                            output = "Closed";
                        }
                    }
                    if (dInfo.deviceType == "Sensor")
                    {
                        roomStatus.basicCurrentAq += dInfo.cAq;
                        roomStatus.basicCurrentHum += dInfo.cHum;
                        roomStatus.basicCurrentTemp += dInfo.cTemp;
                        
                        countD++;
                    }
                }

                
                
            }
            basicDeviceDList.Remove(dModel);

            if (roomStatus.basicWindowStatus == null || output == "")
            {
                roomStatus.basicWindowStatus = output;
            }
            if (countD!= 0)
            {
                roomStatus.basicCurrentAq /= countD;
                roomStatus.basicCurrentHum /= countD;
                roomStatus.basicCurrentTemp /= countD;
            }
            else
            {
                roomStatus.basicCurrentAq = 0;
                roomStatus.basicCurrentHum = 0;
                roomStatus.basicCurrentTemp = 0;
                throw new Exception("Could not get proper average data for room");
            }
            roomStatusList.Add(roomStatus);
        }
        return roomStatusList;
    }
}