using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private const string PLATFORM_PUBLISHED = "Platform_Published";

        private IServiceScopeFactory _serviceScopeFactory;
        private IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    return;
                default:
                    return;
            }
        }

        private void AddPlatform(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                // We cannot directly inject ICommandRepo in constructor
                // because EventProcessor has to be a singleton instance
                // and ICommandRepo has to be scoped (per request).
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);
                    if (repo.ExternalPlatformExists(platform.ExternalId))
                    {
                        Console.WriteLine($"Platform with external id: {platform.ExternalId} already exists");
                        return;
                    }
                    repo.CreatePlatform(platform);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to add platform to DB: {ex.Message}");
                }
            }
        }

        private EventType DetermineEvent(string eventMessage)
        {
            var eventDto = JsonSerializer.Deserialize<GenericEventDto>(eventMessage);
            if (eventDto == null) throw new ArgumentException("Unable to deserialize event message");

            switch (eventDto.Event)
            {
                case PLATFORM_PUBLISHED:
                    Console.WriteLine("Platform published event detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine($"Unknown event type: {eventDto}");
                    return EventType.Unknown;
            }
        }
    }

    enum EventType
    {
        Unknown,
        PlatformPublished
    }
}