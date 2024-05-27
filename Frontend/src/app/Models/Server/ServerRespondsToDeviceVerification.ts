import {BaseDto} from "../objects/baseDto";

export class ServerRespondsToDeviceVerification extends BaseDto<ServerRespondsToDeviceVerification>{
  foundDevice? : Boolean;
  deviceTypeName? : string;
  deviceGuid? : string;
}

