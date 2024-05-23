import {BaseDto} from "../baseDto";


export class DeviceModel {
  deviceTypeName?: string;
  deviceGuid?: string;
}

export class SensorModelDto extends BaseDto<any> {
  deviceTypeName?: string;
  sensorGuid?: string;
}
