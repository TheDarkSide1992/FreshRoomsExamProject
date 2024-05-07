using Infastructure.CostumExeptions;
using Infastructure.DataModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Infastructure;

public class OpenMeteoApi
{
    private HttpClient _client;
    public OpenMeteoApi(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<GeolocationModels?> getlocations(string city)
    {
        try
        {
            var url = $"https://geocoding-api.open-meteo.com/v1/search?name={city.ToLower()}&count=10&language=en&format=json";
            var response = await _client.GetStringAsync(url);
            GeolocationModels locations = JsonSerializer.Deserialize<GeolocationModels>(response);
            return locations;
        }
        catch (Exception e)
        {
            throw new GeolocationExeption("failed to get location");
        }

    }
    

    public async Task<DailyWeatherModel?> getDailyForecast(Location location)
    {
        try
        {
            var url = $"https://api.open-meteo.com/v1/dwd-icon?latitude={location.latitude}&longitude={location.longitude}&&daily=weather_code,temperature_2m_max,apparent_temperature_max,precipitation_probability_max&forecast_days=5";
            var response = await _client.GetStringAsync(url);
            var forecast = JsonSerializer.Deserialize<DailyWeatherModel>(response);
            return forecast;
        }
        catch (Exception e)
        {
            throw new ForcastExeption("failed to get the forecast for today");
        }
    }

    public async Task<TodayWeatherModel?> getTodaysForecast(Location location)
    {
        var url = $"https://api.open-meteo.com/v1/dwd-icon?latitude={location.latitude}&longitude={location.longitude}&hourly=temperature_2m,apparent_temperature,precipitation,weather_code&forecast_days=1&models=icon_eu";
        var response = await _client.GetStringAsync(url);
        var forecast = JsonSerializer.Deserialize<TodayWeatherModel>(response);
        return forecast;
    }
}