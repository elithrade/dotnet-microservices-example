using CommandService.DataSyncServices.Grpc;
using CommandService.Models;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PopulateData(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                if (grpcClient == null) throw new InvalidOperationException("Cannot resolve IPlatformDataClient");
                var platforms = grpcClient.ReturnAllPlatforms();

                var CommandRepo = serviceScope.ServiceProvider.GetService<ICommandRepo>();
                if (CommandRepo == null) throw new InvalidOperationException("Cannot resolve ICommandRepo");

                SeedData(CommandRepo, platforms);

            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");
            foreach (var platform in platforms)
            {
                if (!repo.ExternalPlatformExists(platform.ExternalId))
                    repo.CreatePlatform(platform);
            }

            repo.Commit();
        }
    }
}