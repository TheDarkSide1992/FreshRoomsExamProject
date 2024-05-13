import {BaseDto} from "./baseDto";
import {DeviceModel, DeviceTypesModel} from "./DeviceModel";

export class ServerSendsDeviceTypes extends BaseDto<ServerSendsDeviceTypes>{
  deviceTypeList? : Array<DeviceTypesModel>;
}
