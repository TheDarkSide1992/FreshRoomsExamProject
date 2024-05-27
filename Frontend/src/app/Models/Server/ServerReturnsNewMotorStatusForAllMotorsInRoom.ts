import { BaseDto } from "../objects/baseDto";
import {MotorModel} from "../objects/MotorModel";

export interface ServerReturnsNewMotorStatusForAllMotorsInRoom extends BaseDto<ServerReturnsNewMotorStatusForAllMotorsInRoom> {
  motors: MotorModel[];
  message? : string
}
