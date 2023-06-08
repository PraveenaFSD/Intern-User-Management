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

namespace UserIntern.Controllers
{
    
    
        [Route("api/[controller]")]
        [ApiController]
        [EnableCors("AngularCORS")]
        public class UserController : ControllerBase
        {
            private readonly IManageUser _manageUser;
        private readonly IRepo<int, User> _repo;

        public UserController(IManageUser manageUser, IRepo<int, User> userRepo)
            {
                _manageUser = manageUser;
                _repo=userRepo;
            }
            [HttpPost]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Register(InternDTO intern)
            {
                var result = await _manageUser.Register(intern);
                if (result != null)
                {
                return Created("Home", intern);
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
        [HttpGet("getalluser")]
        [ProducesResponseType(typeof(ICollection<User>), 200)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetAllUser()
        {
            ICollection<User> users = await _repo.GetAll();
            if (users != null)
            {
                return Ok(users);
            }
            return NotFound(new Error(1, "No User Details Currently"));

        }
        [Authorize(Roles = "admin")]
        [HttpGet("get single user")]
        [ProducesResponseType(typeof(ICollection<User>), 200)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetSingleUser(int userId)
        {
            User users = await _repo.Get(userId);
            if (users != null)
            {
                return Ok(users);
            }
            return NotFound(new Error(1, "No User Detail with this id"));

        }


    }
}

