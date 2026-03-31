using DeviceManagement.Config;
using DeviceManagement.model;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.service
{
    class DeviceService : IDeviceService
    {
        SystemDbContext _context;

        public DeviceService(SystemDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            return await _context.Devices.FindAsync(id);
        }

        public async Task<Device> AddDeviceAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<bool> UpdateDeviceAsync(Device device)
        {
            if(!await _context.Devices.AnyAsync(dev => dev.Id == device.Id))
            {
                return false;
            }
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if(device == null)
            {
                return false;
            }
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}