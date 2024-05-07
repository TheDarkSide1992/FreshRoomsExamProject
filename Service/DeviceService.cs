using Infastructure;

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
}