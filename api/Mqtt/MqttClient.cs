using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace api.Mqtt;

public class MqttClient
{
   public IMqttClient _Client;
   public MqttFactory Factory;
    
    public async Task communicateWithMqttBroker()
    {
        Factory = new MqttFactory();
        _Client = Factory.CreateMqttClient();
        
        byte[]? caCertFile = null;
        X509Certificate2? caCert = null;
        caCertFile = File.ReadAllBytes(Environment.GetEnvironmentVariable("crt"));
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
    }

    public async Task subscribeToTopics(List<string> topics)
    {
        var mqttSubscribeOptions = Factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f =>
            {
                foreach (var topic in topics)
                {
                    f.WithTopic(topic);
                }
            })
            .Build();

        await _Client.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
    }


    public async Task sendMessageToTopic(string topic, string message)
    {
        var pongMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            .Build();
        await _Client.PublishAsync(pongMessage, CancellationToken.None);
    }
}