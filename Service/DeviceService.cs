using Infastructure;
using Infastructure.DataModels;

namespace Service;

public class DeviceService
{
    private readonly DeviceRepository _deviceRepository;

    public DeviceService(DeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public void createOrUpdateSensorData(SensorModel sensorModel)
    {
        if (_deviceRepository.checkIfSensorDataExist(sensorModel.sensorId))
        {
            _deviceRepository.saveOldData(sensorModel.sensorId);
        }
        
        _deviceRepository.createOrUpdateData(sensorModel);
    }

    public void updateMotorStatusMQTT(MotorModel motorModel)
    {
        _deviceRepository.updateMoterStatusMQTT(motorModel);
    }

    public RoomAvrageSensorData getAvrageRoomSensorData(string sensorId)
    {
        return _deviceRepository.getAvrageSensordataforRoom(sensorId);
    }

    public List<MotorModel> getMotorsForRoom(int roomId)
    {
        return (List<MotorModel>)_deviceRepository.getMotorsForRoom(roomId);
    }

    public int getRoomIdFromDeviceId(string id)
    {
        return _deviceRepository.getRoomIdFromDeviceId(id);
    }

    public List<SensorModel> getSensorsForRoom(int roomid)
    {
        return _deviceRepository.getSensorsForRoom(roomid);
    }

    public void updateMoterstatusWithUsersInput(MotorModel motorModel)
    {
        _deviceRepository.UpdateMoterModelWithUsersInput(motorModel);
    }

    public List<MotorModel> updateAllMotorsInAroom(int roomid, bool open, bool isDisabled)
    {
        return (List<MotorModel>)_deviceRepository.updateAllMotersInARoom(roomid, open, isDisabled);
    }
}