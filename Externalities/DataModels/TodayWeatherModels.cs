namespace Infastructure.DataModels;

public class TodayWeatherModels
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double generationtime_ms { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public int elevation { get; set; }
    public HourlyUnits hourly_units { get; set; }
    public Hourly hourly { get; set; }
}

public class Hourly
{
    public List<string> time { get; set; }
    public List<double> temperature_2m { get; set; }
    public List<double> apparent_temperature { get; set; }
    public List<double> precipitation { get; set; }
    public List<int> weather_code { get; set; }
}

public class HourlyUnits
{
    public string time { get; set; }
    public string temperature_2m { get; set; }
    public string apparent_temperature { get; set; }
    public string precipitation { get; set; }
    public string weather_code { get; set; }
}

