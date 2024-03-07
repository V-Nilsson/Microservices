using AutoMapper;
using CommandsService.Contract;
using CommandsService.Data;
using CommandsService.Models;
using Serilog;
using System.Text.Json;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IMapper mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        this.scopeFactory = scopeFactory;
        this.mapper = mapper;
    }
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                break;
            default: 
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage) 
    {
        Log.Information("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEvent>(notificationMessage);

        switch (eventType.Event) 
        {
            case nameof(PlatformPublished):
                Log.Information("Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                Log.Information("--> Could not determine event type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

            var platformPublished = JsonSerializer.Deserialize<PlatformPublished>(platformPublishedMessage);

            try
            {
                var platform = mapper.Map<Platform>(platformPublished);
                if (!repository.ExternalPlatformExists(platform.ExternalId))
                {
                    repository.CreatePlatform(platform);
                    repository.SaveChanges();
                }
                else
                {
                    Log.Information("--> Platform already exists");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"--> Could not add platform to Database: {ex.Message}");
            }
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}
