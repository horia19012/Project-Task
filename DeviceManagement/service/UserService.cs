using DeviceManagement.Config;
using DeviceManagement.model;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.service
{
    class UserService : IUserService
    {
        private readonly SystemDbContext _context;

        public UserService(SystemDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> AddUserAsync(User user)
        {
             _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if(!await _context.Users.AnyAsync(u => u.Id == user.Id))
            {
                return null;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}