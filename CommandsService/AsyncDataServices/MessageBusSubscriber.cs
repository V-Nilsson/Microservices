
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using System.Threading.Channels;

namespace CommandsService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration configuration;
    private readonly IEventProcessor eventProcessor;
    private IConnection connection;
    private IModel channel;
    private string queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        this.configuration = configuration;
        this.eventProcessor = eventProcessor;

        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMqHost"],
            Port = int.Parse(configuration["RabbitMqPort"]),
        };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(
            queue: queueName,
            exchange: "trigger",
            routingKey: "");

        Log.Information("--> Listening on the Message Bus");

        connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            Log.Information("--> Event Received");
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            eventProcessor.ProcessEvent(notificationMessage);
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Log.Information("--> Connection Shutdown");
    }

    public override void Dispose()
    {
        if (channel.IsOpen)
        {
            channel.Close();
            connection.Close();
        }

        base.Dispose();
    }
}
