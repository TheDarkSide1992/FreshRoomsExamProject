import {BaseDto} from "../objects/baseDto";

export class ClientWantsToDisableOrEnableAllMotorsFromRoom extends BaseDto<ClientWantsToDisableOrEnableAllMotorsFromRoom>
{
  roomId?: number;
  disable?: boolean;
}
