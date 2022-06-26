using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() >= 0;
        }

        public void Create(int platformId, Command command)
        {
            if (command == null) throw new ArgumentException(nameof(command));

            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null) throw new ArgumentNullException(nameof(platform)); ;

            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public Command? GetCommand(int platformId, int commandId)
        {
            if (_context.Commands == null) return null;

            return _context.Commands
                .Where(x => x.PlatformId == platformId && x.Id == commandId)
                .FirstOrDefault();
        }

        public IEnumerable<Command> GetCommands(int platformId)
        {
            return _context.Commands
                .Where(x => x.PlatformId == platformId)
                .OrderBy(x => x.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(x => x.Id == platformId);
        }
    }
}