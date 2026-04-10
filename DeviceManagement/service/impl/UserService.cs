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
        
        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A list of users.</returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<User> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>The added user.</returns>
        public async Task<User> AddUserAsync(User user)
        {
             _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>The updated user if found; otherwise, null.</returns>

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

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the user was deleted; otherwise, false.</returns>
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