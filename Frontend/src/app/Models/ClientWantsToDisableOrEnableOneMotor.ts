import {BaseDto} from "./baseDto";
import {MotorModel} from "./objects/MotorModel";

export class ClientWantsToDisableOrEnableOneMotor extends BaseDto<ClientWantsToDisableOrEnableOneMotor>
{
  motor?: MotorModel;
  disable?: boolean;
  roomId?: number;
}
