import {BaseDto} from "./baseDto";

export class ClientWantsToOpenOrCloseAllWindowsInRoom extends BaseDto<ClientWantsToOpenOrCloseAllWindowsInRoom>
{
  id?: number
  open?: boolean
}
