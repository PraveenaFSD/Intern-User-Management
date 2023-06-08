using System.Security.Cryptography;
using System.Text;
using UserIntern.Interfaces;
using UserIntern.Models.DTO;
using UserIntern.Models;

namespace UserIntern.Services
{
    public class ManageUserServices:IManageUser
    {
        private readonly IRepo<int, User> _userRepo;
        private readonly IRepo<int, Intern> _internRepo;
        private readonly IGeneratePassword _passwordService;
        private readonly ITokenGenerate _tokenService;

        public ManageUserServices(IRepo<int, User> userRepo,
            IRepo<int, Intern> internRepo,
            IGeneratePassword passwordService,
            ITokenGenerate tokenService)
        {
            _userRepo = userRepo;
            _internRepo = internRepo;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }
        public Task<UserDTO> ChangeStatus(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> Login(UserDTO user)
        {
            UserDTO userDetails = null;
            var userData = await _userRepo.Get(user.UserId);
            if(userData!=null && userData.Status.Equals("abled"))
            {
                var hmac = new HMACSHA512(userData.PasswordKey);
                var password = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] != userData.PasswordHash[i])
                    {
                        return null;
                    }
                }
                user = new UserDTO();
                user.UserId = user.UserId;
                user.Role = user.Role;
                user.Token = _tokenService.GenerateToken(user);

            }
            return user;
        }

        public async Task<UserDTO> Register(InternDTO intern)
        {

            UserDTO user = null;
            var hmac = new HMACSHA512();
            string? generatedPassword = await _passwordService.GeneratePassword(intern);
            intern.User.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(generatedPassword ?? intern.Name+"123"));
            intern.User.PasswordKey = hmac.Key;
            var userResult = await _userRepo.Add(intern.User);
            var internResult = await _internRepo.Add(intern);
            if (userResult != null && internResult != null)
            {
                user = new UserDTO();
                user.UserId = internResult.Id;
                user.Role = userResult.Role;
                user.Token = _tokenService.GenerateToken(user);
            }
            return user;

        }
    }

}
