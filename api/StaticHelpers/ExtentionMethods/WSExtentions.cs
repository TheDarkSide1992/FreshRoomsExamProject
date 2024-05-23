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

    public static void AddClientToRoom(this IWebSocketConnection ws, int roomId)
    {
        if (!WebSocketConnections.usersInrooms.ContainsKey(roomId))
        {
            WebSocketConnections.usersInrooms.TryAdd(roomId, new HashSet<Guid>());
        }
        WebSocketConnections.usersInrooms[roomId].Add(ws.ConnectionInfo.Id);
    }

    public static void AddDeviceId(this IWebSocketConnection ws, string deviceId)
    {
        if (!WebSocketConnections.deviceVerificationList.ContainsKey(deviceId))
        {
            WebSocketConnections.deviceVerificationList.TryAdd(deviceId, new HashSet<Guid>());
        }
        if(!WebSocketConnections.deviceVerificationList[deviceId].Contains(ws.ConnectionInfo.Id))
            WebSocketConnections.deviceVerificationList[deviceId].Add(ws.ConnectionInfo.Id);
    }

    public static void RemoveDeviceId(this IWebSocketConnection ws ,string deviceId)
    {
        if (WebSocketConnections.deviceVerificationList.ContainsKey(deviceId))
        {
            WebSocketConnections.deviceVerificationList.TryRemove(deviceId, out _);
        }
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