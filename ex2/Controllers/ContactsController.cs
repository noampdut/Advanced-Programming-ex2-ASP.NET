using System;
using System.Collections.Generic;
using System.Linq;
using ex2.Models;
using ex2.Services;
using Microsoft.AspNetCore.Mvc;

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
        //static List<Contact> contactList = new List<Contact> { };

        public ContactsController(IUsersService usersService)
        {
            userService = usersService;
            //List<Contact> contactList = new List<Contact> { };
            //Message message = new Message() { id = 1, created = "today", sent = true, content = "yesss" };
            //List<Message> listMessages = new List<Message> { message };
            //contactList.Add(new Contact { id = "Lilach", lastDate = "today", last = "by", name = "lilach", messages = listMessages, server = "fds"});
            //user = new User() { Id = "NoamPdut", NickName = "Noamit", Password = "n123456", Picture = "", Contacts = contactList };
            //userService = new UserService(tempUser);
            contactsService = new ContactService(userService.GetActiveUser().Contacts);
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
        public IActionResult Index()
        {
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
        //[ValidateAntiForgeryToken]
        public IActionResult Create(string id, string name, string server)
        {
            if (contactsService.Get(id) == null && userService.GetActiveUser().Id != id)
            {
                contactsService.Add(name, id, server);
                return StatusCode(201);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Details(string id)
        {
            Contact contact = contactsService.Get(id);

            return Json(fixContact(contact));
        }
        [HttpPut("{id}")]
        public IActionResult Edit(string id, string name, string server)
        {
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
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            bool returnValue = contactsService.Delete(id);
            if (returnValue == false)
            {
                return NotFound();
            } else
            {
                return StatusCode(204);
            }
        }

        // Get: api/contacts/id/messages
        [HttpGet("{id}/Messages"), ActionName("Messages")]
        public IActionResult getMessages(string id)
        {
            Contact contact = contactsService.Get(id);
            return Json(contact.messages);
        }

        [HttpPost("{id}/Messages"), ActionName("Messages")]
        public IActionResult createMessage(string id, string content)
        {
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
            return StatusCode(201);
        }

        // Get: api/contacts/id/messages/id2
        [HttpGet("{id1}/Messages/{id2}"), ActionName("Messages")]
        public IActionResult getMessage(string id1, int id2)
        {
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
        [HttpPut("{id1}/Messages/{id2}"), ActionName("Messages")]
        public IActionResult editMessage(string id1, int id2, string content)
        {
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
        [HttpDelete("{id1}/Messages/{id2}"), ActionName("Messages")]
        public IActionResult deleteMessage(string id1, int id2) {
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
