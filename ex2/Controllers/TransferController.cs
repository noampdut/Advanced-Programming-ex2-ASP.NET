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
    public class TransferController : Controller
    {
        private IUsersService userService;
        private IHubContext<MyHub> hubContext;
        public TransferController(IUsersService user_Service, IHubContext<MyHub> hubContext)
        {
            userService = user_Service;
            this.hubContext = hubContext;
        }
        [HttpPost]
        public async Task<IActionResult> Index(string from, string to, string content)
        {
            User user = userService.Get(to);
            if(user == null)
            {
                return NotFound();
            }
            if(user.Contacts == null)
            {
                return NotFound();
            }
            Contact contact = user.Contacts.Find(contact => contact.id == from);
            if (contact == null)
            {
                return NotFound();
            }
            int nextId = 0;
            if (contact.messages.Count != 0)
            {
                nextId = contact.messages.Max(x => x.id) + 1;
            }
            else
            {
                nextId = 1;
            }
            string Date = DateTime.Now.ToString();
            Message message = new Message() { id = nextId, content = content, sent = true, Created = Date };
            contact.messages.Add(message);
            contact.last = message.content;
            contact.lastDate = message.Created;

            await hubContext.Clients.All.SendAsync("getNewMessage");
            return StatusCode(201);
        }
        
    }
}
