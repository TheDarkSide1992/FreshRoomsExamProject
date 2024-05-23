import { BaseDto } from "../objects/baseDto";
import {DetailedRoomModel} from "../objects/DetailedRoomModel";

export class ServerReturnsDetailedRoomToUser extends BaseDto<ServerReturnsDetailedRoomToUser> {
  room?: DetailedRoomModel;
}
