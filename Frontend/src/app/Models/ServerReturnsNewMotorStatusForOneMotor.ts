import {BaseDto} from "./baseDto";
import {MotorModel} from "./objects/MotorModel";

export class ServerReturnsNewMotorStatusForOneMotor extends BaseDto<ServerReturnsNewMotorStatusForOneMotor>
{
  motor?: MotorModel;
  message?: string;
}
