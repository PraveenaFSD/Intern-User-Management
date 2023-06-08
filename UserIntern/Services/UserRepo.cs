using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserIntern.Interfaces;
using UserIntern.Models;
using UserIntern.Models.DTO;

namespace UserIntern.Services
{
    public class UserRepo : IRepo<int, User>
    {
        private readonly UserContext _context;
        private readonly ILogger<UserRepo> _logger;

        public UserRepo(UserContext context, ILogger<UserRepo> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User?> Add(User item)
        {
            try
            {
                _context.Users.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public async Task<User?> Delete(int key)
        {
            var user = await Get(key);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public async Task<User?> Get(int key)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == key);
            return user;
        }

        public async Task<ICollection<User>?> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count > 0)
                return users;
            return null;
        }

        public async Task<User?> Update(User item)
        {
            try
            {
                var user = await Get(item.UserId);
                if (user != null)
                {
                    user.Role = item.Role;
                    user.PasswordHash = item.PasswordHash;
                    user.PasswordKey = item.PasswordKey;
                    user.Status = item.Status;
                    await _context.SaveChangesAsync();
                    return user;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return null;





        }
    }
}
    
