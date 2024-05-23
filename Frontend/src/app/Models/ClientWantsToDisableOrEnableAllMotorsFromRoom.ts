import {BaseDto} from "./baseDto";

export class ClientWantsToDisableOrEnableAllMotorsFromRoom extends BaseDto<ClientWantsToDisableOrEnableAllMotorsFromRoom>
{
  roomId?: number;
  disable?: boolean;
}
