using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using api.State;
using api.StaticHelpers.ExtentionMethods;
using Infastructure.DataModels;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Formatter;
using Newtonsoft.Json;
using Serilog;
using Service;
using socketAPIFirst.Dtos;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace api.Mqtt;

public class MqttClient(DeviceService _deviceService, RoomService _roomService)
{
   public IMqttClient _client;
   public MqttFactory factory;
   public DateTime? timelastChecked = null;

   /*
    * this method handles all communication with the mqtt broker
    */
   public async Task communicateWithMqttBroker()
   {
       factory = new MqttFactory();
       _client = factory.CreateMqttClient();

       byte[]? caCertFile = null;
       X509Certificate2? caCert = null;
       caCertFile = File.ReadAllBytes(Environment.GetEnvironmentVariable("crt")!);
       caCert = new X509Certificate2(caCertFile);

       var mqttClientOptions = new MqttClientOptionsBuilder()
           .WithTcpServer(Environment.GetEnvironmentVariable("mqtt_host"), 8883)
           .WithProtocolVersion(MqttProtocolVersion.V500)
           .WithTlsOptions(
               opts =>
               {
                   opts.WithAllowUntrustedCertificates();
                   opts.WithSslProtocols(SslProtocols.Tls12);
                   opts.WithCertificateValidationHandler(certconf =>
                   {
                       var chain = new X509Chain();
                       chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                       chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
                       chain.ChainPolicy.CustomTrustStore.Add(caCert);
                       chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;

                       return chain.Build(new X509Certificate2(certconf.Certificate));
                   });
               })
           .Build();
       await _client.ConnectAsync(mqttClientOptions, CancellationToken.None);

       var mqttsensorsub = factory.CreateSubscribeOptionsBuilder()
           .WithTopicFilter(f => f.WithTopic("freshrooms/sensor/#"))
           .Build();
       
       var mqttmotorsub = factory.CreateSubscribeOptionsBuilder()
           .WithTopicFilter(f => f.WithTopic("freshrooms/motor/status/#"))
           .Build();
       var mqttdeviceverification = factory.CreateSubscribeOptionsBuilder()
           .WithTopicFilter(f => f.WithTopic("freshrooms/verified/#"))
           .Build();
       await _client.SubscribeAsync(mqttsensorsub, CancellationToken.None);
       await _client.SubscribeAsync(mqttmotorsub, CancellationToken.None);
       await _client.SubscribeAsync(mqttdeviceverification, CancellationToken.None);
       Console.WriteLine("mqtt connection successfull");
       _client.ApplicationMessageReceivedAsync += async e =>
       {
           try
           {
               var message = e.ApplicationMessage.ConvertPayloadToString();
               if (e.ApplicationMessage.Topic.ToString().Split("/")[1].Equals("sensor"))
               {
                   var roomid = _deviceService.getRoomIdFromDeviceId(e.ApplicationMessage.Topic.ToString().Split("/")[2]);
                   var obj = JsonConvert.DeserializeObject<SensorModel>(message);
                   obj.sensorId = e.ApplicationMessage.Topic.ToString().Split("/")[2];
                   sendSensorDataToAllUsersInRooms(roomid, obj);
                   saveOrCreateSensordata(message, e.ApplicationMessage.Topic.ToString().Split("/")[2]);
               }
               else if (e.ApplicationMessage.Topic.ToString().Split("/")[1].Equals("motor") &&
                        e.ApplicationMessage.Topic.ToString().Split("/")[2].Equals("status"))
               {
                   createMotorStatus(message,e.ApplicationMessage.Topic.ToString().Split("/")[3] );
               }
               else if (e.ApplicationMessage.Topic.ToString().Split("/")[1].Equals("verified"))
               {
                   sendVerificationToUsers(e.ApplicationMessage.Topic.ToString().Split("/")[2], message);
               }
           }
           catch (Exception ex)
           {
               Log.Error(ex.Message);
               throw new MqttCommunicationException("failed to work with message");
           }
       };
       
   }
    


    public async Task sendMessageToTopic(string topic, string message)
    {
        var pongMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            .Build();
        await _client.PublishAsync(pongMessage, CancellationToken.None);
    }

    private void saveOrCreateSensordata(string message, string id)
    {
        var obj = JsonConvert.DeserializeObject<SensorModel>(message);
        obj.sensorId = id;
        _deviceService.createOrUpdateSensorData(obj);
        if (timelastChecked != null && DateTime.Now.Minute >= timelastChecked.Value.Minute + 5)
        {
            openWindows(id);
        }
        if (timelastChecked == null)
        {
            timelastChecked = DateTime.Now;
        }
        
    }

    public void openWindows(string id)
    {
        var avrage = _deviceService.getAverageRoomSensorData(id);
        var pref = _roomService.getRoomPrefrencesConfiguration(avrage.roomId);
        List<MotorModel> motors = _deviceService.getMotorsForRoom(avrage.roomId);
        if (avrage.Humidity <= pref.minHumidity || avrage.Humidity >= pref.maxHumidity)
        {
            windowAction(motors);
        }
        else if(avrage.Temperature <= pref.minTemparature || avrage.Temperature >= pref.maxTemparature)
        {
            windowAction(motors);
        }
        else if (avrage.CO2 <= pref.minAq || avrage.CO2 >= pref.maxAq)
        {
            windowAction(motors);   
        }
    }

    private void windowAction(List<MotorModel> motors)
    {
        foreach (var m in motors)
        {
            if (!m.isDisabled)
            {
                if (!m.isOpen)
                {
                    sendMessageToTopic("freshrooms/motor/action/" + m.motorId, "open");
                    m.isOpen = true;
                    _deviceService.updateMotorStatusMQTT(m);
                }
                else
                {
                    sendMessageToTopic("freshrooms/motor/action/" + m.motorId, "close");
                    m.isOpen = false;
                    _deviceService.updateMotorStatusMQTT(m);
                }
            }
        }

        timelastChecked = DateTime.Now;
    }

    public void openAllWindowsWithUserInput(List<MotorModel> motors, bool open,int roomid)
    {
        var message = "";
        foreach (var m in motors)
        {
            if (open)
            {
                sendMessageToTopic("freshrooms/motor/action/" + m.motorId, "open");
                m.isOpen = true;
                m.isDisabled = true;
                _deviceService.updateMotorstatusWithUsersInput(m);
                message = "all windows are open or are being opened";
            }
            else
            {
                sendMessageToTopic("freshrooms/motor/action/" + m.motorId, "close");
                m.isOpen = false;
                m.isDisabled = false;
                _deviceService.updateMotorstatusWithUsersInput(m);
                message = "all windows are closed or are being closed";
            }
        }
        sendMotorDataForAllMotorsToAllUsersInRooms(roomid ,motors, message);
    }
    
    private void createMotorStatus(string message, string id)
    {
        var obj = JsonSerializer.Deserialize<MotorModel>(message);
        obj.motorId = id;
        _deviceService.updateMotorStatusMQTT(obj);
    }

    public void sendSensorDataToAllUsersInRooms(int id, SensorModel model)
    {
        if ( WebSocketConnections.usersInrooms.TryGetValue(id, out var guids))
        {
            foreach (var guid in guids)
            {
                if(WebSocketConnections.connections.TryGetValue(guid, out var ws))
                {
                    ws.Socket.Send(JsonSerializer.Serialize(new ServerReturnsNewestSensorData{data = model}));
                }
            }
        }
    }
    
    public void sendMotorDataForAllMotorsToAllUsersInRooms(int id, List<MotorModel> motors, string message)
    {
        if ( WebSocketConnections.usersInrooms.TryGetValue(id, out var guids))
        {
            foreach (var guid in guids)
            {
                if(WebSocketConnections.connections.TryGetValue(guid, out var ws))
                {
                    ws.Socket.Send(JsonSerializer.Serialize(new ServerReturnsNewMotorStatusForAllMotorsInRoom(){motors = motors, message = message}));
                }
            }
        }
    }

    public void openOrCloseAWindow(MotorModel motor, bool open)
    {
        if (open)
        {
            sendMessageToTopic("freshrooms/motor/action/" + motor.motorId, "open");
        }
        else
        {
            sendMessageToTopic("freshrooms/motor/action/" + motor.motorId, "close");
        }
    }

    public void verifyDeviceGuid(string guid)
    {
        sendMessageToTopic("freshrooms/verify/" + guid, "verify");
    }
    
    public void sendVerificationToUsers(string deviceId, string deviceType)
    {
        
        if ( WebSocketConnections.deviceVerificationList.TryGetValue(deviceId, out var guid))
        {
                if(WebSocketConnections.connections.TryGetValue(guid, out var ws))
                {
                    ws.Socket.Send(JsonSerializer.Serialize(new ServerRespondsToDeviceVerificationDto
                    {
                        foundSensor = true,
                        deviceTypeName = deviceType,
                        sensorGuid = deviceId
                    }));
                    ws.Socket.removeDeviceId(deviceId);
                }
        }
    }
}