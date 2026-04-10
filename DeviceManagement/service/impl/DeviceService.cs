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
        /// <summary>
        /// Retrieves all devices assigned to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of devices assigned to the user.</returns>
        public async Task<IEnumerable<Device>> GetDevicesByUserIdAsync(int userId)
        {
            return await _context.Devices
                .Where(device => device.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a device by its ID.
        /// </summary>
        /// <param name="id">The ID of the device.</param>
        /// <returns>The device if found; otherwise, null.</returns>
        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            return await _context.Devices.FindAsync(id);
        }

        /// <summary>
        /// Adds a new device to the database.
        /// </summary>
        /// <param name="device">The device to add.</param>
        /// <returns>The added device.</returns>
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

        /// <summary>
        /// Updates an existing device in the database.
        /// </summary>
        /// <param name="device">The device to update.</param>
        /// <returns>True if the device was updated; otherwise, false.</returns>
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
    
        /// <summary>
        /// Deletes a device from the database.
        /// </summary>
        /// <param name="id">The ID of the device to delete.</param>
        /// <returns>True if the device was deleted; otherwise, false.</returns>
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
        /// <summary>
        /// Assigns a device to a user by updating the UserId property of the device
        /// </summary>
        /// <param name="id">The ID of the device to assign</param>
        /// <param name="userId">The ID of the user to assign the device to</param>
        /// <returns>The updated device if the assignment was successful; otherwise, null.</returns>
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
        /// <summary>
        /// Unassigns a device from a user by setting the UserId property of the device to null.
        /// </summary>
        /// <param name="id">The ID of the device to unassign</param>
        /// <returns>The updated device if the unassignment was successful, otherwise, null</returns>
        public async Task<Device> UnassignFromUser(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return null;
            }

            device.UserId = null;
            _context.Update(device);
            await _context.SaveChangesAsync();
            return device;
        }
    }
}