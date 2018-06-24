using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

namespace TestSend
{
    class Program
    {
        static void Main(string[] args)
        {

            Task.Run(async () =>
            {

                var options = new MqttClientOptionsBuilder()
                    .WithClientId("Client1")
                    .WithTcpServer("10.0.0.1")
                    .WithCredentials("bud", "%spencer%")
                    .WithCleanSession()
                    .Build();
                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();

                await mqttClient.ConnectAsync(options);
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("MyTopic")
                    .WithPayload("Hello World")
                    .WithExactlyOnceQoS()
                    .WithRetainFlag()
                    .Build();

                await mqttClient.PublishAsync(message);
                Console.ReadLine();
            }).Wait();
        }
         // sre_SpeechRecognized
    }
}
