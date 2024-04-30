using Fleck;
using Infastructure.DataModels;

namespace api.Models;

public class WebSocketMetadata
{
    public IWebSocketConnection? Socket {get; set;}
    public bool isAuthenticated {get; set;}
    public User userInfo {get; set;}
}