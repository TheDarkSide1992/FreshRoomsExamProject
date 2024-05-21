import {BaseDto} from "./baseDto";
import {MotorModel} from "./objects/MotorModel";

export class ClientWantsToOpenOrCloseWindow extends BaseDto<ClientWantsToOpenOrCloseWindow>
{
  roomId?: number;
  motor?: MotorModel;
  open?: boolean;
}
