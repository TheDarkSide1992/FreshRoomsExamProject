using Infastructure.DataModels;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsForecast : BaseDto
{
    public DailyWeatherModel dailyForecast { get; set; }
    public TodayWeatherModel todaysForecast { get; set; }
    public string tempAndUnitPrefrence { get; set; }
}