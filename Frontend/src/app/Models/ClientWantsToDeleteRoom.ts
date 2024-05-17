import {BaseDto} from "./baseDto";

export class ClientWantsToDeleteRoom extends BaseDto<ClientWantsToDeleteRoom>{
  roomId?: number;
  roomName?: string;
}
