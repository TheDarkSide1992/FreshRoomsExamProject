import { BaseDto } from "./baseDto";
import {DetailedRoomModel} from "./objects/DetailedRoomModel";

export class ServerReturnsDetailedRoomToUser extends BaseDto<ServerReturnsDetailedRoomToUser> {
  room?: DetailedRoomModel;
}
