using api.Models;
using api.State;
using Fleck;
using Infastructure.DataModels;

namespace api.StaticHelpers.ExtentionMethods;

public static class WSExtentions
{
    public static void AddConnection(this IWebSocketConnection ws)
    {
        WebSocketConnections.connections.TryAdd(ws.ConnectionInfo.Id,
            new WebSocketMetadata
            {
                Socket = ws
            });
    }

    public static void RemoveFromConnections(this IWebSocketConnection connection)
    {
        WebSocketConnections.connections.TryRemove(connection.ConnectionInfo.Id, out _);
    }

    public static void Authenticate(this IWebSocketConnection connection, User userinfo)
    {
        var metadata = connection.GetMetadata();
        metadata.isAuthenticated = true;
        metadata.userInfo = userinfo;
    }
    
    public static void UnAuthenticate(this IWebSocketConnection connection)
    {
        var metadata = connection.GetMetadata();
        metadata.isAuthenticated = false;
    }

    public static WebSocketMetadata GetMetadata(this IWebSocketConnection connection)
    {
        return WebSocketConnections.connections[connection.ConnectionInfo.Id];
    }
}