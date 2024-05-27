using System.Text.Json;
using api.Dtos;
using api.EventFilters;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using Infastructure.DataModels;
using lib;
using Service;
using socketAPIFirst.Dtos;

namespace api.ClientRequest;

[AuthenticationRequired]
[ValidateDataAnnotations]
public class clientWantsToGetWeatherForcast(AccountService accountService, OpenMeteoService openMeteo) : BaseEventHandler<clientWantsToGetWeatherForcastDto>
{
    public override async Task Handle(clientWantsToGetWeatherForcastDto dto, IWebSocketConnection socket)
    {
        var metadata = socket.getMetadata();
        var city = accountService.getCity(metadata.userInfo.userId);
        var location = await openMeteo.getlocation(city);
        var dailyForcast = await openMeteo.getDailyForecast(location);
        var todaysForecast = await openMeteo.getTodaysForecast(location);

        socket.Send(JsonSerializer.Serialize(new ServerReturnsForecast{dailyForecast = dailyForcast, todaysForecast = todaysForecast}));
    }
}