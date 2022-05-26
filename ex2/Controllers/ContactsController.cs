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
    public class ContactsController : Controller
    {
        //private IUserService userService;
        //private User user;
        private IUsersService userService;
        private IContactService contactsService;
        private IHubContext<MyHub> hubContext;
        //static List<Contact> contactList = new List<Contact> { };

        public ContactsController(IUsersService usersService, IHubContext<MyHub> hubContext)
        {
            userService = usersService;
            this.hubContext = hubContext;
            //List<Contact> contactList = new List<Contact> { };
            //Message message = new Message() { id = 1, created = "today", sent = true, content = "yesss" };
            //List<Message> listMessages = new List<Message> { message };
            //contactList.Add(new Contact { id = "Lilach", lastDate = "today", last = "by", name = "lilach", messages = listMessages, server = "fds"});
            //user = new User() { Id = "NoamPdut", NickName = "Noamit", Password = "n123456", Picture = "", Contacts = contactList };
            //userService = new UserService(tempUser);
            //if (userService.GetActiveUser() != null)
            // {
            //    contactsService = new ContactService(userService.GetActiveUser().Contacts);

            //}
        }
        private dynamic fixContact(Contact contact)
        {
            if (contact == null)
            {
                return (new { });
            }
            var temp = new
            {
                id = contact.id,
                name = contact.name,
                server = contact.server,
                last = contact.last,
                lastDate = contact.lastDate
            };
            return temp;
        }
        [HttpGet]
        //api/contacts/user
        public IActionResult Index(string user)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            List<Contact> contactsList = contactsService.GetAll();
            List<dynamic> returnContacts = new List<dynamic> { };
            if (contactsList != null)
            {
                for (int i = 0; i < contactsList.Count; i++)
                {
                    returnContacts.Add(fixContact(contactsList[i]));
                }
                return Json(returnContacts);
            }else {
                return Json(new EmptyResult());
            }    
        }

        [HttpPost]
        //create new contact
        //api/contacts?userId/contactName/contactNickName/contactServer
        public async Task<IActionResult> Create(string user,string id, string name, string server)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            if (contactsService.Get(id) == null && activeUser.Id != id)
            {
                contactsService.Add(name, id, server);
                await hubContext.Clients.All.SendAsync("newContactInList");
                return StatusCode(201);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        //api/contacts?userId/contactName
        public IActionResult Details(string user, string id)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            Contact contact = contactsService.Get(id);
            return Json(fixContact(contact));
        }
        [HttpPut("{id}")]
        //api/contacts?userId/contactName/contactNickName/contactServer
        public IActionResult Edit(string user, string id, string name, string server)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            Contact temp = contactsService.Get(id);
            if (temp != null)
            {
                temp.name = name;
                temp.server = server;
                contactsService.Edit(temp);
                return StatusCode(204);
            } else {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        //api/contacts?userId/contactName
        public IActionResult Delete(string user, string id)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            bool returnValue = contactsService.Delete(id);
            if (returnValue == false)
            {
                return NotFound();
            } else
            {
                return StatusCode(204);
            }
        }

        // Get: api/contacts/user/id/messages
        [HttpGet("{id}/Messages"), ActionName("Messages")]
        public IActionResult getMessages(string id,string user)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            //contactsService = new ContactService(activeUser.Contacts);
            //Contact contact = contactsService.Get(id);
           // return Json(contact.messages);
            return Json(activeUser.Contacts.Find(x => x.id == id).messages);
        }

        [HttpPost("{user}/{id}/Messages"), ActionName("Messages")]
        public async Task<IActionResult> createMessage(string user, string id, string content)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            Contact contact = contactsService.Get(id);
            if (contact == null)
            {
                return NotFound();
            }
            string Date = DateTime.Now.ToString();
            int nextId;
            if (contact.messages.Count != 0)
            {
                nextId = contact.messages.Max(x => x.id) + 1;
            }
            else
            {
                nextId = 1;
            }
            Message message = new Message() { id = nextId, content = content, created = Date, sent = false };
            contact.messages.Add(message);
            contact.last = message.content;
            contact.lastDate = message.created;
            await hubContext.Clients.All.SendAsync("getNewMessage");
            return StatusCode(201);
        }

        // Get: api/contacts/id/messages/id2
        [HttpGet("{user}/{id1}/Messages/{id2}"), ActionName("Messages")]
        public IActionResult getMessage(string user, string id1, int id2)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            Contact contact = contactsService.Get(id1);
            if (contact != null)
            {
                Message message = contact.messages.Find(x => x.id == id2);
                if (message != null)
                {
                    return Json(message);
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();
        }

        // Put: api/contacts/id/messages/id2
        [HttpPut("{user}/{id1}/Messages/{id2}"), ActionName("Messages")]
        public IActionResult editMessage(string user, string id1, int id2, string content)
        {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            Contact contact = contactsService.Get(id1);
            if (contact != null)
            {
                Message message = contact.messages.Find(x => x.id == id2);
                if (message != null)
                {
                    message.content = content;
                    message.created = DateTime.Now.ToString();
                    return StatusCode(204);
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();
            
        }

        // Delete: api/contacts/id/messages/id2
        [HttpDelete("{user}/{id1}/Messages/{id2}"), ActionName("Messages")]
        public IActionResult deleteMessage(string user, string id1, int id2) {
            User activeUser = userService.Get(user);
            if (activeUser == null)
            {
                return NotFound();
            }
            contactsService = new ContactService(activeUser.Contacts);
            Contact contact = contactsService.Get(id1);
            if (contact != null)
            {
                Message message = contact.messages.Find(x => x.id == id2);
                if (message != null)
                {
                    contact.messages.Remove(message);
                    return StatusCode(204);

                }
                return NotFound();
            }
            return NotFound();
        }
    }
}
