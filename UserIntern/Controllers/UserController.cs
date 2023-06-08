using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UserIntern.Interfaces;
using UserIntern.Models;
using UserIntern.Models.DTO;
using UserIntern.Services;

namespace UserIntern.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AngularCORS")]
    public class UserController : ControllerBase
    {
        private readonly IManageUser _manageUser;
        private readonly IRepo<int, Intern> _internRepo;
        private readonly IRepo<int, User> _userRepo;

        public UserController(IManageUser manageUser, IRepo<int, Intern> internRepo,IRepo<int,User> userRepo)
        {
            _manageUser = manageUser;
            _internRepo = internRepo;
            _userRepo=userRepo;
           
        }
        [HttpPost("Register User")]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Register(InternDTO intern)
        {
            var result = await _manageUser.Register(intern);
            if (result != null)
            {
                return Created("Home", result);
            }
            return BadRequest(new Error(2, "Unable to register user at this moment"));
        }

        [HttpPost("Login User")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Login([FromBody] UserDTO userDTO)
        {
            UserDTO user = await _manageUser.Login(userDTO);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest(new Error(2, "Cannot Login user. Password or username may be incorrect or user may be not registered"));


        }
        [Authorize(Roles = "admin")]
        [HttpGet("Get All Intern")]
        [ProducesResponseType(typeof(ICollection<Intern>), 200)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Intern>> GetAllInterns()
        {
            ICollection<Intern> interns = await _internRepo.GetAll();
            if (interns != null)
            {
                return Ok(interns);
            }
            return NotFound(new Error(1, "No Intern Details Currently"));

        }
      
        [Authorize(Roles = "admin")]
        [HttpGet("Get Single Intern")]
        [ProducesResponseType(typeof(ICollection<Intern>), 200)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Intern>> GetSingleIntern(int id)
        {
            Intern intern = await _internRepo.Get(id);
            if (intern != null)
            {
                return Ok(intern);
            }
            return NotFound(new Error(1, "No intern Detail with this id"));

        }
        [Authorize(Roles = "admin")]
        [HttpGet("Get All Users")]
        [ProducesResponseType(typeof(IList<AllUserDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AllUserDTO>> GetAllUsers()
        {
            IList<AllUserDTO> users = await _manageUser.GetAllUserDetails();
            if (users != null)
            {
                return Ok(users);
            }
            return NotFound(new Error(1, "No user Details Currently"));

        }
        [Authorize(Roles = "admin")]
        [HttpGet("Get Single User")]
        [ProducesResponseType(typeof(AllUserDTO), 200)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<AllUserDTO>> GetSingleUser(int id)
        {
            AllUserDTO user = await _manageUser.GetSingleUserDetails(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound(new Error(1, "No user Detail with this id"));

        }



        [Authorize]
        [HttpPut("Update User Password")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> UpdatePassword([FromBody] UserDTO user)
        {
            User userData = await _userRepo.Get(user.UserId);
            if (userData == null)
                return NotFound(new Error(1, "No such user is present"));
            UserDTO userDetails = await _manageUser.ChangePassword(user);
            if (userDetails != null)
            {
                return Accepted("Updated User Password successfully") ;
            }
            return BadRequest(new Error(2, "Cannot Update User Password"));
        }
        [Authorize(Roles ="admin")]
        [HttpPut("Update User status")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> UpdateUserStatus([FromBody] UserStatus user)
        {
            User userData = await _userRepo.Get(user.Id);
            if (userData == null)
                return NotFound(new Error(1, "No such user is present"));
            UserStatus userDetails = await _manageUser.ChangeStatus(user);
            if (userDetails != null)
            {
                return Accepted("Updated user status successfully");
            }
            return BadRequest(new Error(2, "Cannot Update User Status"));
        }



    }
}

