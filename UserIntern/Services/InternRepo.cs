using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UserIntern.Interfaces;
using UserIntern.Models;

namespace UserIntern.Services
{
    public class InternRepo : IRepo<int, Intern>
    {
        private readonly UserContext _context;
        private readonly ILogger<InternRepo> _logger;

        public InternRepo(UserContext context, ILogger<InternRepo> logger)
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

        public async Task<Intern?> Get(int key)
        {
            var interns = await _context.Interns.FirstOrDefaultAsync(u => u.Id == key);
            return interns;
        }

        public async Task<ICollection<Intern>?> GetAll()
        {
            var interns = await _context.Interns.ToListAsync();
            if (interns.Count > 0)
                return interns;
            return null;
        }

        public Task<Intern?> Update(Intern item)
        {
            throw new NotImplementedException();
        }
    }
}

