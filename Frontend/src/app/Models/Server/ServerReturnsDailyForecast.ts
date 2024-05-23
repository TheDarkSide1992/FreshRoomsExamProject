import {BaseDto} from "../objects/baseDto";
import {DailyWeatherModel} from "../objects/DailyForcastModels";
import {TodayWeatherModel} from "../objects/TodaysForcastModels";

export class ServerReturnsForecast extends BaseDto<ServerReturnsForecast>
{
  dailyForecast? : DailyWeatherModel;
  todaysForecast? : TodayWeatherModel;
}
