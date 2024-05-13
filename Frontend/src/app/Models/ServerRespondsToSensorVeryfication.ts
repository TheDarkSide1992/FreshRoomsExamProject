import {BaseDto} from "./baseDto";

export class ServerRespondsToSensorVeryfication extends BaseDto<ServerRespondsToSensorVeryfication>{
  foundSensor? : Boolean;
  deviceTypeName? : string;
  sensorGuid? : string;
}

