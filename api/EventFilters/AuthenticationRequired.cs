using System.Security.Authentication;
using api.StaticHelpers.ExtentionMethods;
using Fleck;
using lib;

namespace api.EventFilters;

public class AuthenticationRequired: BaseEventFilter
{
    public override Task Handle<T>(IWebSocketConnection socket, T dto)
    {
        if (!socket.getMetadata().isAuthenticated)
        {
            throw new AuthenticationException("you are not logged in please login and try again");
        }
        else
        {
            return Task.CompletedTask;
        }
    }
}