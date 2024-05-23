import {BaseDto} from "../objects/baseDto";

export class ClientWantsToOpenOrCloseAllWindowsInRoom extends BaseDto<ClientWantsToOpenOrCloseAllWindowsInRoom>
{
  id?: number
  open?: boolean
}
