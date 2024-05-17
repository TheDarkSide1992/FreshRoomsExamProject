import {BaseDto} from "./baseDto";
import {DeviceModel} from "./DeviceModel";

export class ServerReturnsCreatedRoom extends BaseDto<ServerReturnsCreatedRoom> {
  roomId?: number;
  name?: string;
  creatorId?: number;
  deviceList?: Array<DeviceModel>;
}
