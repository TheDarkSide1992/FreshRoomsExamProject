import {BaseDto} from "./baseDto";
import {BasicRoomStatusModel} from "./objects/BasicRoomStatusModel";

export class ServerReturnsBasicRoomStatus extends BaseDto<ServerReturnsBasicRoomStatus>{
  basicRoomListData?: Array<BasicRoomStatusModel>
}
