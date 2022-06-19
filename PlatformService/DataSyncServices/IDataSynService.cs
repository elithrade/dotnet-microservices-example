using PlatformService.Dtos;

namespace PlatformService.DataSyncServices
{
    public interface IDataSyncService
    {
        Task SendPlatform(PlatformReadDto platform);
    }
}