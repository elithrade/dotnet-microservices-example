using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        bool Commit();

        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatform(Platform platform);

        bool PlatformExists(int platformId);

        bool ExternalPlatformExists(int externalPlatformId);

        IEnumerable<Command> GetCommands(int platformId);

        Command? GetCommand(int platformId, int commandId);

        void Create(int platformId, Command command);
    }
}