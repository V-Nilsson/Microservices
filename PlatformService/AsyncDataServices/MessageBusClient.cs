using PlatformService.Contract;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration configuration;
    private readonly IConnection connection;
    private readonly IModel channel;

    public MessageBusClient(IConfiguration configuration)
    {
        this.configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMqHost"],
            Port = int.Parse(configuration["RabbitMqPort"])
        };
        try
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;

            Log.Information("--> Connected to Message Bus");
        }
        catch (Exception ex)
        {
            Log.Error($"--> Could not connect to the message bus: {ex.Message}");
        }
    }
    public void PublishNewPlatform(PlatformPublished platform)
    {
        var message = JsonSerializer.Serialize(platform);

        if (connection.IsOpen) 
        {
            Log.Information("--> RabbitMq Connection Open, sending message");
            SendMessage(message);
        }

    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "trigger", 
            routingKey: "",
            basicProperties: null,
            body: body);
        Log.Information($"--> We have sent {message}");
    }

    public void Dispose()
    {
        Log.Information("Message Bus Disposed");
        if (channel.IsOpen)
        {
            channel.Close();
            connection.Close();
        }
    }

    private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
    {
        Log.Information("--> RabbitMq Connection Shutdown");
    }
}
