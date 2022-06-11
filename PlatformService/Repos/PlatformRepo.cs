using PlatformService.Context;
using PlatformService.Models;

namespace PlatformService.Repo
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() >= 0;
        }

        public void Create(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException();
            }

            _context.Platforms.Add(platform);
        }

        public Platform Get(int id)
        {
            return _context.Platforms.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Platform> GetAll()
        {
            return _context.Platforms.AsEnumerable();
        }
    }
}