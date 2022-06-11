using PlatformService.Context;
using PlatformService.Models;

namespace PlatformService.Repo
{
    public static class PrepDb
    {
        public static void PopulateDummyData(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext? appDbContext)
        {
            if (appDbContext == null)
            {
                throw new ArgumentNullException("DbContext is not defined.");
            }

            var platforms = appDbContext.Platforms;
            if (platforms.Any())
            {
                Console.WriteLine("Data already seeded");
                return;
            }

            Console.WriteLine("Seeding DB with dummy data...");

            platforms.AddRange(
                new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

            appDbContext.SaveChanges();
        }
    }
}