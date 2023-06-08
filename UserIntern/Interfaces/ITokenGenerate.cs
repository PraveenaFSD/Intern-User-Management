using UserIntern.Models.DTO;

namespace UserIntern.Interfaces
{
    public interface ITokenGenerate

    {
        public string GenerateToken(UserDTO user);

    }
}
