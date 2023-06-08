using System.Security.Cryptography;
using System.Text;
using UserIntern.Interfaces;
using UserIntern.Models.DTO;
using UserIntern.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
        public async Task<UserStatus> ChangeStatus(UserStatus user)
        {
            User userUpdate;
            var userData =await _userRepo.Get(user.Id);
            if(userData != null)
            {
                userUpdate = new User();
                userData.Status=user.Status;
                
                if(await _userRepo.Update(userData) != null)
                {
                    UserStatus userResult = new UserStatus();
                    userResult.Id = user.Id;
                   
                    return userResult;
                }
            }
            return null;
        }

        public async Task<UserDTO> Login(UserDTO user)
        {
            UserDTO userDetails = null;
            var userData = await _userRepo.Get(user.UserId);
            if(userData!=null)
            {
                if(userData.Role.Equals("admin".ToLower()) ||(userData.Role.Equals("intern".ToLower()) && userData.Status.Equals("abled".ToLower())))
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
                    userDetails = new UserDTO();
                    userDetails.UserId = user.UserId;
                    userDetails.Role = user.Role;
                    userDetails.Token = _tokenService.GenerateToken(userDetails);
                }
               

            }
            return userDetails;
        }

        public async Task<UserDTO> Register(InternDTO intern)
        {

            UserDTO user = null;
            var hmac = new HMACSHA512();
            string? generatedPassword = await _passwordService.GeneratePassword(intern);
            Debug.WriteLine(generatedPassword);
            intern.User.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(generatedPassword ?? "" ));
            intern.User.PasswordKey = hmac.Key;
            intern.User.Status = "disabled";
            var userResult = await _userRepo.Add(intern.User);
            if (intern.User.Role != "admin".ToLower())
            {
                var internResult = await _internRepo.Add(intern);
            }
            if (userResult != null )
            {
                user = new UserDTO();
                user.UserId = userResult.UserId;
                user.Role = userResult.Role;
                user.Token = _tokenService.GenerateToken(user);
                return user;
            }
            return null;
            

        }
        public async Task<UserDTO> ChangePassword(UserDTO user)
        {

            User userData = await _userRepo.Get(user.UserId);
            if (userData != null)
            {
                var hmac = new HMACSHA512();

                userData.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                userData.PasswordKey = hmac.Key;
                var updateduser =await _userRepo.Update(userData);
                if (updateduser != null)
                {
                    UserDTO userResult = new UserDTO();
                    userResult.UserId = userData.UserId;
                    userResult.Role = userData.Role;
                    return userResult;

                }


            }

            return null;
        }



    }

}
