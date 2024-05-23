import {BaseDto} from "../objects/baseDto";


export class ClientWantsToUpdateRoomConf extends BaseDto<ClientWantsToUpdateRoomConf>{
  roomId? : number
  updatedMinTemperature? : number
  updatedMaxTemperature? : number
  updatedMaxHumidity? : number
  updatedMinHumidity? : number
  updatedMinAq? : number
  updatedMaxAq? : number
}
