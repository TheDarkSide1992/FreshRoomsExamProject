import {BaseDto} from "./baseDto";

export class ServerReturnsBasicWindowStatus extends BaseDto<any>{
  windowStatus?: string;
  roomId?: number;
}
