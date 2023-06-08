using System.Diagnostics;
using UserIntern.Interfaces;
using UserIntern.Models;

namespace UserIntern.Services
{
    public class InternRepo : IRepo<int, Intern>
    {
        private readonly UserContext _context;
        private readonly ILogger<UserRepo> _logger;

        public InternRepo(UserContext context, ILogger<UserRepo> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Intern?> Add(Intern user)
        {
         

            try
            {
                _context.Interns.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public Task<Intern?> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public Task<Intern?> Get(int key)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Intern>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Intern?> Update(Intern item)
        {
            throw new NotImplementedException();
        }
    }
}

