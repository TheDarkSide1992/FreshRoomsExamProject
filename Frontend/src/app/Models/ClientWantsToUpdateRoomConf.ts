import {BaseDto} from "./baseDto";


export class ClientWantsToUpdateRoomConf extends BaseDto<ClientWantsToUpdateRoomConf>{
  roomId? : number
  updatedMinTemperature? : number
  updatedMaxTemperature? : number
  updatedMaxHumidity? : number
  updatedMinHumidity? : number
  updatedMinAq? : number
  updatedMaxAq? : number
}
