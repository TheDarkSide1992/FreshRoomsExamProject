using api.CostumExeptions;
using Fleck;
using lib;

namespace api.EventFilters;

public class rateLimiter(int requestsPerMinute): BaseEventFilter
{
    private static readonly Dictionary<Guid, DateTime> Timestamps = new();
    private static readonly Dictionary<Guid, int> RequestCounts = new();
    
    public override async Task Handle<T>(IWebSocketConnection socket, T dto)
    {
        var guid = socket.ConnectionInfo.Id;

        if (!Timestamps.ContainsKey(guid))
        {
            InitializeRequestCountAndTimeStamp(guid);
            return;
        }

        if ((DateTime.Now - Timestamps[guid]).TotalMinutes >= 1)
        {
            ResetRequestCountAndTimeStamp(guid);
            return;
        }

        IncrementRequestCount(guid);
        if (RequestCounts[guid] > requestsPerMinute)
        {
            TooManyRequests(socket);
        }
    }

    /*
     * returns the HttpStatusCode TooManyRequests
     */
    private void TooManyRequests(IWebSocketConnection socket)
    {
        throw new TooManyRequestsExeption("too many requests received please wait a bit and try again");
    }

    /*
     * initializes the request counter and timestamp
     */
    private void InitializeRequestCountAndTimeStamp(Guid guid)
    {
        Timestamps[guid] = DateTime.Now;
        RequestCounts[guid] = 1;
    }

    /*
     * resets the request counter and timestamp
     */
    private void ResetRequestCountAndTimeStamp(Guid guid)
    {
        Timestamps[guid] = DateTime.Now;
        RequestCounts[guid] = 1;
    }

    /*
     * adds 1 to the request count
     */
    private void IncrementRequestCount(Guid guid)
    {
        RequestCounts[guid] += 1;
    }
}