using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ex2.Models;
using ex2.Services;
using Microsoft.AspNetCore.Mvc;

namespace ex2.Controllers
{
    [ApiController]
    [Route("api/Register")]
    public class RegisterController : Controller
    {
        private IUsersService userService;
        public RegisterController(IUsersService user_Service)
        {
            userService = user_Service;
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User request)
        {
            if (request == null)
            {
                return BadRequest("Invalid registration data.");
            }

            if (userService.isSigned(request.Id))
            {
                return StatusCode(409, "Username is already taken.");
            }

            var user = new User
            {
                Id = request.Id,
                NickName = request.NickName,
                Password = request.Password, // Ideally, passwords should be hashed and salted
                Picture = request.Picture,
                
            };

            userService.Add(user.Id, user.NickName, user.Password, "1234", user.Picture);
            return Ok(user);
        }

       

        private dynamic fixUser(User user)
        {
            if (user == null)
            {
                return (new { });
            }
            var temp = new
            {
                userName = user.Id,
                nickName = user.NickName,
                picture = " ",
                contacts = user.Contacts
                    
            };
            return temp;
        }
    }
}
