import {BaseDto} from "../objects/baseDto";

export class ServerLogsInUser extends BaseDto<ServerLogsInUser>
{
  jwt? : string;
}
