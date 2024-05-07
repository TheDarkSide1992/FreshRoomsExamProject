using api;
using Fleck;
using lib;
using Serilog;
using api.Middleware;
using System.Reflection;
using api.StaticHelpers.ExtentionMethods;
using Infastructure;
using Service;

public static class StartUp
{
    public static void Main(String[] args)
    {
        var app = StatUp(args);
        app.Run();
    }

    public static WebApplication StatUp(String[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "\n{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}\n")
            .CreateLogger();

        var server = new WebSocketServer("ws://0.0.0.0:8181");
        var builder = WebApplication.CreateBuilder(args);
        var clientEventHandler = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());
    
        builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
            dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
        builder.Services.AddSingleton<AccountRepository>();
        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<HashRepository>();
        builder.Services.AddSingleton<DeviceService>();
        builder.Services.AddSingleton<DeviceRepository>();
        builder.Services.AddSingleton<OpenMeteoApi>();
        builder.Services.AddSingleton<OpenMeteoService>();
        builder.Services.AddSingleton<HttpClient>();
       
        var app = builder.Build();

        void Config(IWebSocketConnection ws)
        {
            ws.OnOpen = ws.AddConnection;
            ws.OnClose = ws.RemoveFromConnections;
            ws.OnError = ex => ex.Handle(ws, null);
            ws.OnMessage = async message =>
            {
                try
                {
                    await app.InvokeClientEventHandler(clientEventHandler, ws, message);
                }
                catch (Exception ex)
                {
                    ex.Handle(ws, message);
                }
            };
        }

        server.RestartAfterListenError = true;
        server.Start(Config);
        return app;
    }
}
