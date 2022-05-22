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
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private IUsersService userService;
        public RegisterController(IUsersService user_Service)
        {
            userService = user_Service;
        }
        [HttpPost]
        public IActionResult Index(string userName, string nickName, string pwd)
        {
            bool returnValue = userService.isSigned(userName);
            if (!returnValue)
            {
                userService.Add(userName, nickName, pwd, "1234");
                userService.setActiveUser(userName);
                return Json(fixUser(userService.GetActiveUser()));
            }
            else
            {
                return StatusCode(404);
            }
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
