using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ex2.Hubs;
using ex2.Models;
using ex2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ex2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : Controller
    {
        private IUsersService userService;
        private IHubContext<MyHub> hubContext;
        public InvitationsController(IUsersService user_Service, IHubContext<MyHub> hubContext)
        {
            userService = user_Service;
            this.hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string from, string to, string server)
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
            await hubContext.Clients.All.SendAsync("newContactInList");
            return StatusCode(201);
        }
        
    }
}
