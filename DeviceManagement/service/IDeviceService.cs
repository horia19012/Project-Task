using DeviceManagement.model;

namespace DeviceManagement.service
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<IEnumerable<Device>> GetDevicesByUserIdAsync(int userId);
        Task<Device> GetDeviceByIdAsync(int id);
        Task<Device> AddDeviceAsync(Device device);
        Task<bool> UpdateDeviceAsync(Device device);
        Task<bool> DeleteDeviceAsync(int id);
        Task<Device> AssignToUser(int id, int userId);
        Task<Device> UnassignFromUser(int id);
    }    
}
