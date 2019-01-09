using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JwtAPI.Services;
using JwtAPI.Models;

namespace JwtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        // GET api/auth/protected
        [Authorize]
        [HttpGet("protected")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // Used for logging in and generating the token
        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Post([FromBody]User userParams)
        {
            var token = _userService.Login(userParams.Username, userParams.Password);

            //user not found return 401
             if (token == null)
                return Unauthorized();
            
            // user found return the token with 200 status.
            return Ok(new {Jwt = token});
        }

    }
}
