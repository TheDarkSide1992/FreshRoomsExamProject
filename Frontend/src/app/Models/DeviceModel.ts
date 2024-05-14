import {BaseDto} from "./baseDto";


export class DeviceModel {
  deviceTypeName?: string;
  sensorGuid?: string;
}

export class SensorModelDto extends BaseDto<any> {
  deviceTypeName?: string;
  sensorGuid?: string;
}

export class DeviceTypesModel {
  deviceTypeId?: number;
  deviceTypeName?: string;
}
