import {BaseDto} from "../Models/baseDto";


export class SensorModel  {
  name?: string;
  sensorGuid?: string;
}

export class SensorModelDto extends BaseDto<any> {
  name?: string;
  sensorGuid?: string;
}
