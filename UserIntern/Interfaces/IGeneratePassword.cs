using UserIntern.Models;

namespace UserIntern.Interfaces
{
    public interface IGeneratePassword
    {
        public Task<string?> GeneratePassword(Intern intern);

    }
}
