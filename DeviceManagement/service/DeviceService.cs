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
            if (device.UserId.HasValue)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == device.UserId.Value);
                if (!userExists)
                {
                    throw new ArgumentException($"User with ID {device.UserId.Value} does not exist.");
                }
            }
            if (_context.Devices.Any(d => d.Name == device.Name && d.Manufacturer == device.Manufacturer && device.UserId == null))
            {
                throw new ArgumentException("A device with the same name and manufacturer already exists. Just assign it to a user!");
            }
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<bool> UpdateDeviceAsync(Device device)
        {
            if (!await _context.Devices.AnyAsync(dev => dev.Id == device.Id))
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
            if (device == null)
            {
                return false;
            }
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Device> AssignToUser(int id, int userId)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return null;
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return null;
            }

            device.UserId = userId;
            _context.Update(device);
            await _context.SaveChangesAsync();
            return device;
        }
    }
}