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

    public string VerifySensorGuid(String sensorGuid)
    {
        return _deviceRepository.VerifySensorGuid(sensorGuid);
    }

    public IEnumerable<DeviceTypeModel> getSensorTypes()
    {
        return _deviceRepository.getDeviceTypes();
    }

    public void createOrUpdateSensorData(SensorModel sensorModel)
    {
        _deviceRepository.saveOldData(sensorModel.sensorId);
        _deviceRepository.createOrUpdateData(sensorModel);
    }

    public bool updateMotorStatusMQTT(MotorModel motorModel)
    {
        return _deviceRepository.updateMoterStatusMQTT(motorModel);
    }

    public RoomAvrageSensorData getAvrageRoomSensorData(string sensorId)
    {
        return _deviceRepository.getAvrageSensordataforRoom(sensorId);
    }

    public List<MotorModel> getMotersForRoom(int roomId)
    {
        return (List<MotorModel>)_deviceRepository.getMotersForRoom(roomId);
    }

    public int getRoomIdFromDeviceId(string id)
    {
        return _deviceRepository.getRoomIdFromDeviceId(id);
    }

    public List<SensorModel> getSensorsForRoom(int roomid)
    {
        return _deviceRepository.getSensorsForRoom(roomid);
    }
}