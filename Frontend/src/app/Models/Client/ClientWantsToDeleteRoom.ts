import {BaseDto} from "../objects/baseDto";

export class ClientWantsToDeleteRoom extends BaseDto<ClientWantsToDeleteRoom>{
  roomId?: number;
  roomName?: string;
}
