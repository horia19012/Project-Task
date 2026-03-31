using DeviceManagement.model;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Config
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {
        }
    public DbSet <Device> Devices { get; set; }

    }
}