using api.Models;
using api.State;
using Fleck;

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
    
}