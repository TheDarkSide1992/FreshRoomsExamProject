import { BaseDto } from "./baseDto";
import {SensorModel} from "./objects/SensorModel";

export interface ServerReturnsNewestSensorData extends BaseDto<ServerReturnsNewestSensorData> {
  data: SensorModel;
}
