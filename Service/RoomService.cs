using System.Security.Authentication;
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
            _deviceRepository.updateDevices(deviceList, roomModel.roomId);
            _deviceRepository.createMoterStatusList(deviceList);
            return roomModel;
        }
        return null;
    }

    public bool DeleteRoom(int roomId)
    {
        if (_roomRepository.DeleteRoomConfig(roomId))
        {
            _deviceRepository.deleteHistoricDataForRoom(roomId);
            _deviceRepository.deleteCurrentDataForRoom(roomId);
            _deviceRepository.deleteMotorStatus(roomId);
            if (_deviceRepository.deleteRoomIdOnDevices(roomId))
            {
                return _roomRepository.DeleteRoom(roomId);
            }
        }
        else
        {
            throw new Exception("Could not delete room");
        }

        return false;
    }
    
    public RoomConfigModel getRoomPrefrencesConfiguration(int dtoRoomId)
    {
        return _roomRepository.getRoomPreferencesConfiguration(dtoRoomId);
    }


   
    public RoomConfigModel updateRoomPrefrencesConfiguration(int userInfoUserId, int dtoRoomId, double dtoUpdatedMinTemperature, double dtoUpdatedMaxTemperature, double dtoUpdatedMinHumidity, double dtoUpdatedMaxHumidity, double dtoUpdatedMinAq, double dtoUpdatedMaxAq)
    {
        if (_accountRepository.isAdmin(userInfoUserId))
        {
            return _roomRepository.updateRoomPreferencesConfiguration( dtoRoomId, dtoUpdatedMinTemperature, dtoUpdatedMaxTemperature, dtoUpdatedMinHumidity, dtoUpdatedMaxHumidity, dtoUpdatedMinAq, dtoUpdatedMaxAq);
        }
        throw new AuthenticationException("You are not admin, how did you even get this exception???");
    }
    
    public string getRoomName(int roomid)
    {
        return _roomRepository.getRoomName(roomid);
    }

    /**
     * This method handles the data returned from the DB and merges it into one object for each room,
     * then returns a list of BasicRoomStatus objects for all rooms found in the DB 
     */
    public IEnumerable<BasicRoomStatus> getBasicRoomWindowStatus()
    {
        try
        {
            List<BasicRoomStatus> roomStatusList = new List<BasicRoomStatus>();
        IEnumerable<BasicRoomSettingModel> roomConfigModels = _roomRepository.GetALLRoomSettings();
        List<BasicDeviceDModel> basicDeviceDList = _deviceRepository.getBasicDeviceData();
        
        foreach (var roomConfig in roomConfigModels)
        {
            
            BasicRoomStatus roomStatus = new BasicRoomStatus();
            int countD = 0;
            string output = "";
            BasicDeviceDModel dModel = null;
            foreach (var dInfo in basicDeviceDList)
            {
                if (roomConfig.roomId == dInfo.roomId)
                {
                    dModel = dInfo;
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
                    if (dInfo.deviceType == "Sensor" && dInfo.cTemp != null)
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
                roomStatus.basicCurrentTemp = Math.Round(roomStatus.basicCurrentTemp/countD, 2);
                roomStatus.basicCurrentHum = Math.Round(roomStatus.basicCurrentHum/countD, 2);
                roomStatus.basicCurrentAq = Math.Round(roomStatus.basicCurrentAq/countD, 2);
            }
            else
            {
                roomStatus.basicCurrentAq = 0;
                roomStatus.basicCurrentHum = 0;
                roomStatus.basicCurrentTemp = 0;
                
            }
            roomStatusList.Add(roomStatus);
        }
        return roomStatusList;
        }
        catch (Exception e)
        {
            throw new Exception("Could not get proper average data for room");
        }
    }
}