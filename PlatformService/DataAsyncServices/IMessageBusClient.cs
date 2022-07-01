using PlatformService.Dtos;

namespace PlatformService.DataAsyncServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }
}