using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions opt) : base(opt)
        {
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}