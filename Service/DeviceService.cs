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

    public bool VerifySensorGuid(String sensorGuid)
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

    public void createOrUpdateMotorStatus(MotorModel motorModel)
    {
        _deviceRepository.createOrUpdateMoterStatus(motorModel);
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

    public void updateMoterstatus(MotorModel motorModel)
    {
        _deviceRepository.UpdateMoterModel(motorModel);
    }
}