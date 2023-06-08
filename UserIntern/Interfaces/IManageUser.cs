using UserIntern.Models.DTO;

namespace UserIntern.Interfaces
{
    public interface IManageUser
    {
        public Task<UserDTO> Login(UserDTO user);
        public Task<UserDTO> Register(InternDTO intern);
        public Task<UserStatus> ChangeStatus(UserStatus user);
        public Task<UserDTO> ChangePassword(UserDTO user);

    }
}
