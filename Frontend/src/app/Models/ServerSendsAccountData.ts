import {BaseDto} from "./baseDto";

export class ServerSendsAccountData extends BaseDto<ServerSendsAccountData>{
  email? : string
  city? : string
  realName?: string
}
