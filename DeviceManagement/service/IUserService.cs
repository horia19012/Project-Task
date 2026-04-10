using DeviceManagement.model;
namespace DeviceManagement.service
{
    public interface IUserService
    {
        
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);

    }
}