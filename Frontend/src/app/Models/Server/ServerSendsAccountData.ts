import {BaseDto} from "../objects/baseDto";

export class ServerSendsAccountData extends BaseDto<ServerSendsAccountData>{
  email? : string
  city? : string
  realname?: string
}
