import {BaseDto} from "./baseDto";

export class ServerSendsRoomConfigurations extends BaseDto<ServerSendsRoomConfigurations>{
  minTemparature? : number
  maxTemparature? : number
  maxHumidity? : number
  minHumidity? : number
  minAq? : number
  maxAq? : number
}

