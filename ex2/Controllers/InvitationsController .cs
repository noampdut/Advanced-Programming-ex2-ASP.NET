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
    public class InvitationsController : Controller
    {
        private IUsersService userService;
        public InvitationsController(IUsersService user_Service)
        {
            userService = user_Service;
        }
        [HttpPost]
        public IActionResult Index(string from, string to, string server)
        {
            User user = userService.Get(to);
            if(user == null)
            {
                return NotFound();
            }
            if(user.Contacts == null)
            {
                user.Contacts = new List<Contact>();
            }

            user.Contacts.Add(new Contact() { id = from, name = from, last = "", lastDate = "", server = server, messages = new List<Message> { } });
            return StatusCode(201);
        }
        
    }
}
