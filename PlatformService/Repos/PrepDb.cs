using Microsoft.EntityFrameworkCore;
using PlatformService.Context;
using PlatformService.Models;

namespace PlatformService.Repo
{
    public static class PrepDb
    {
        public static void PopulateDummyData(IApplicationBuilder builder, bool applyMigration)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), applyMigration);
            }
        }

        private static void SeedData(AppDbContext? appDbContext, bool applyMigration)
        {
            if (appDbContext == null)
            {
                throw new ArgumentNullException("DbContext is not defined.");
            }

            if (applyMigration)
            {
                try
                {
                    appDbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not run migration: {ex.Message}");
                }
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