using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            public UserController(IManageUser manageUser)
            {
                _manageUser = manageUser;
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
    }
    }

