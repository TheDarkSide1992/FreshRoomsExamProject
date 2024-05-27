import {DeviceModel} from "./DeviceModel";
import {BaseDto} from "./baseDto";

export class RoomModel {
  roomId?: number;
  name?: string;
  creatorId?: number;
  deviceList? : Array<DeviceModel>;
  basicWindowStatus?: string;
}

export class RoomModelDto extends BaseDto<RoomModelDto>{
  roomId?: number;
  name?: string;
  creatorId?: number;
  deviceList? : Array<DeviceModel>;
}
