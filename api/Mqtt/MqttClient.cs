using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Infastructure.DataModels;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Formatter;
using Newtonsoft.Json;
using Serilog;
using Service;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace api.Mqtt;

public class MqttClient(DeviceService _service)
{
   public IMqttClient _Client;
   public MqttFactory Factory;

   public async Task communicateWithMqttBroker()
   {
       Factory = new MqttFactory();
       _Client = Factory.CreateMqttClient();

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
       await _Client.ConnectAsync(mqttClientOptions, CancellationToken.None);

       var mqttsensorsub = Factory.CreateSubscribeOptionsBuilder()
           .WithTopicFilter(f => f.WithTopic("freshrooms/sensor/#"))
           .Build();
       
       var mqttmotorsub = Factory.CreateSubscribeOptionsBuilder()
           .WithTopicFilter(f => f.WithTopic("freshrooms/motor/status/#"))
           .Build();
       await _Client.SubscribeAsync(mqttsensorsub, CancellationToken.None);
       await _Client.SubscribeAsync(mqttmotorsub, CancellationToken.None);
       Console.WriteLine("mqtt connection successfull");
       _Client.ApplicationMessageReceivedAsync += async e =>
       {
           try
           {
               var message = e.ApplicationMessage.ConvertPayloadToString();
               Console.WriteLine("topic: " + e.ApplicationMessage.Topic.ToString().Split("/")[2]);
               Console.WriteLine("message: " + message);
               if (e.ApplicationMessage.Topic.ToString().Split("/")[1].Equals("sensor"))
               {
                   saveOrCreateSensordata(message, e.ApplicationMessage.Topic.ToString().Split("/")[2]);
               }
               else if (e.ApplicationMessage.Topic.ToString().Split("/")[1].Equals("motor") &&
                        e.ApplicationMessage.Topic.ToString().Split("/")[2].Equals("status"))
               {
                   saveOrCreateMotorStatus(message,e.ApplicationMessage.Topic.ToString().Split("/")[3] );
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
        await _Client.PublishAsync(pongMessage, CancellationToken.None);
    }

    private void saveOrCreateSensordata(string message, string id)
    {
        var obj = JsonConvert.DeserializeObject<SensorModel>(message);
        obj.sensorId = id;
        _service.createOrUpdateSensorData(obj);
    }
    
    private void saveOrCreateMotorStatus(string message, string id)
    {
        var obj = JsonSerializer.Deserialize<MotorModel>(message);
        _service.createOrUpdateMotorStatus(obj);
    }
}