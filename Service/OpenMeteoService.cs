﻿using System.Globalization;
using Infastructure;
using Infastructure.DataModels;

namespace Service;

public class OpenMeteoService
{
    private readonly OpenMeteoApi _openMeteo;
    public OpenMeteoService(OpenMeteoApi openMeteo)
    {
        _openMeteo = openMeteo;
    }
    
    public async Task<Location> getlocation(string city)
    {
        var loc = await _openMeteo.getlocations(city);
        return new Location{longitude = loc.results[0].longitude, latitude = loc.results[0].latitude};
    }

    public async Task<DailyWeatherModel> getDailyForecast(Location location)
    {
        var newtime = new List<string>();
        var forecast = await _openMeteo.getDailyForecast(location);
        forecast.daily.time.RemoveAt(0);
        forecast.daily.apparent_temperature_max.RemoveAt(0);
        forecast.daily.precipitation_probability_max.RemoveAt(0);
        forecast.daily.weather_code.RemoveAt(0);
        forecast.daily.temperature_2m_max.RemoveAt(0);
        foreach (var time in forecast.daily.time)
        {
            var split = time.Split("-");
            newtime.Add(split[2] + "/" + split[1] + " " + split[0]);
        }

        forecast.daily.time = newtime;
        return forecast;
    }

    public async Task<TodayWeatherModel> getTodaysForecast(Location location)
    {
        var unfilteredForecast = await _openMeteo.getTodaysForecast(location);
        var filteredforecast = filterTodaysforcast(unfilteredForecast);
        return filteredforecast;
    }

    public TodayWeatherModel filterTodaysforcast(TodayWeatherModel model)
    {
        var indexes = new List<int>();
        foreach (var time in model.hourly.time)
        {
            DateTime dateTime = DateTime.ParseExact(time, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            if (dateTime.Hour < DateTime.Now.Hour)
            {
                indexes.Add(model.hourly.time.IndexOf(time));
            }
        }

        for (var i = indexes.Count - 1; i >= 0; i--)
        {
            var index = indexes[i];
            model.hourly.time.RemoveAt(index);
            model.hourly.weather_code.RemoveAt(index);
            model.hourly.apparent_temperature.RemoveAt(index);
            model.hourly.precipitation.RemoveAt(index);
            model.hourly.temperature_2m.RemoveAt(index);
        }
        return model;
    }
}