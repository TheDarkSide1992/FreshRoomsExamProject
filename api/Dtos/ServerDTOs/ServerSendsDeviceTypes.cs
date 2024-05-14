using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerSendsDeviceTypes : BaseDto
{
    public IEnumerable<DeviceTypeModel> deviceTypeList { get; set; }
}