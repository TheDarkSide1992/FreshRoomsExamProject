import {BaseDto} from "./baseDto";

export class ServerLogsInUser extends BaseDto<ServerLogsInUser>
{
  jwt? : string;
}
