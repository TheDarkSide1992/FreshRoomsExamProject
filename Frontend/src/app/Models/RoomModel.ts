import {DeviceModel} from "./DeviceModel";
import {BaseDto} from "./baseDto";

export class RoomModel extends BaseDto<RoomModel>{
  name?: string;
  deviceList? : Array<DeviceModel>;
}
