import {SensorModel} from "./SensorModel";
import {MotorModel} from "./MotorModel";

export interface DetailedRoomModel {
  roomId: number;
  name: string;
  sensors?: SensorModel[];
  motors?: MotorModel[];
}
