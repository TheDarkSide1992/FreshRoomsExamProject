import { BaseDto } from "./baseDto";
import {SensorModel} from "./objects/SensorModel";

export class ServerReturnsNewestSensorData extends BaseDto<ServerReturnsNewestSensorData> {
  data?: SensorModel;
}
