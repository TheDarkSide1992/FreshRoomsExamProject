import {BaseDto} from "./baseDto";
import {RoomModelDto} from "./RoomModel";

export class ServerReturnsRoomList extends BaseDto<ServerReturnsRoomList> {
  roomList? : Array<RoomModelDto>;
}
