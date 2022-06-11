using PlatformService.Models;

namespace PlatformService.Repo
{
    public interface IPlatformRepo
    {
        bool Commit();

        IEnumerable<Platform> GetAll();

        Platform Get(int id);

        void Create(Platform platform);
    }
}