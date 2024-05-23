import {BaseDto} from "./baseDto";
import {RoomModelDto} from "./objects/RoomModel";

export class ServerReturnsRoomList extends BaseDto<ServerReturnsRoomList> {
  roomList? : Array<RoomModelDto>;
}
