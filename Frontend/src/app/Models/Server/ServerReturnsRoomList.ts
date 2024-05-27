import {BaseDto} from "../objects/baseDto";
import {RoomModelDto} from "../objects/RoomModel";

export class ServerReturnsRoomList extends BaseDto<ServerReturnsRoomList> {
  roomList? : Array<RoomModelDto>;
}
