import {BaseDto} from "../objects/baseDto";

export class ServerRespondsToDeviceVerification extends BaseDto<ServerRespondsToDeviceVerification>{
  foundSensor? : Boolean;
  deviceTypeName? : string;
  sensorGuid? : string;
}

