using CommandService.Models;

namespace CommandService.DataSyncServices.Grpc
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}