using System.Collections.Concurrent;
using api.Models;

namespace api.State;

public static class WebSocketConnections
{
    public static readonly ConcurrentDictionary<Guid, WebSocketMetadata> connections = new();
    public static readonly ConcurrentDictionary<int, HashSet<Guid>> usersInrooms = new();
}