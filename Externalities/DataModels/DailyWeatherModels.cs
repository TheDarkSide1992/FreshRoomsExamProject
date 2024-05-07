namespace Infastructure.DataModels;

public class DailyWeatherModel
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double generationtime_ms { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public double elevation { get; set; }
    public DailyUnits daily_units { get; set; }
    public Daily daily { get; set; }
}

public class Daily
{
    public List<string> time { get; set; }
    public List<int> weather_code { get; set; }
    public List<double> temperature_2m_max { get; set; }
    public List<double> apparent_temperature_max { get; set; }
    public List<object> precipitation_probability_max { get; set; }
}

public class DailyUnits
{
    public string time { get; set; }
    public string weather_code { get; set; }
    public string temperature_2m_max { get; set; }
    public string apparent_temperature_max { get; set; }
    public string precipitation_probability_max { get; set; }
}
