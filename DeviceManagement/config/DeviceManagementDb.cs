using DeviceManagement.model;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Config
{
    public class DeviceManagementDb : DbContext
    {
        public DeviceManagementDb(DbContextOptions<DeviceManagementDb> options) : base(options)
        {
        }
    public DbSet <Device> Devices { get; set; }

    }
}