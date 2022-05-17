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
        private User user;
        private IContactService contactsService;
        static List<Contact> contactList = new List<Contact> { };

        public ContactsController()
        {
            //List<Contact> contactList = new List<Contact> { };
            Message message = new Message() { Id = 1, Date = "today", sent = true, Content = "yesss" };
            List<Message> listMessages = new List<Message> { message };
            contactList.Add(new Contact { Id = "Lilach", LastDate = "today", LastMessage = "by", NickName = "lilach", Picture = "", messages = listMessages, Service = "fds"});
            user = new User() { Id = "NoamPdut", NickName = "Noamit", Password = "n123456", Picture = "", Contacts = contactList };
            //userService = new UserService(tempUser);
            contactsService = new ContactService(user.Contacts);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Json(contactsService.GetAll());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(string nickName, string id, string service)
        {
            contactsService.Add(nickName, id, service);
            return Json(new EmptyResult());
        }

        [HttpGet("{id}")]
        public IActionResult Details(string id)
        {
            return Json(contactsService.Get(id));
        }
        [HttpPut("{id}")]
        public void Edit( string nickName, string id, string service)
        {
            Contact temp = contactsService.Get(id);
            temp.NickName = nickName;
            temp.Service = service;
            contactsService.Edit(temp);
        }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public void Delete(string id)
        {
            contactsService.Delete(id);
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
                return Json(new EmptyResult());
            }
            string Date = DateTime.Now.ToString();
            int nextId;
            if (contact.messages.Count != 0)
            {
                nextId = contact.messages.Max(x => x.Id) + 1;
            }
            else
            {
                nextId = 1;
            }
            Message message = new Message() { Id = nextId, Content = content, Date = Date, sent = true };
            contact.messages.Add(message);
            return Json(new EmptyResult());
        }

        // Get: api/contacts/id/messages/id2
        [HttpGet("{id1}/Messages/{id2}"), ActionName("Messages")]
        public IActionResult getMessage(string id1, int id2)
        {
            Contact contact = contactsService.Get(id1);
            Message message = contact.messages.Find(x => x.Id == id2);

            return Json(message);
        }

        // Put: api/contacts/id/messages/id2
        [HttpPut("{id1}/Messages/{id2}"), ActionName("Messages")]
        public void editMessage(string id1, int id2, string content)
        {
            Contact contact = contactsService.Get(id1);
            Message message = contact.messages.Find(x => x.Id == id2);
            message.Content = content;
            message.Date = DateTime.Now.ToString();
        }

        // Delete: api/contacts/id/messages/id2
        [HttpDelete("{id1}/Messages/{id2}"), ActionName("Messages")]
        public void deleteMessage(string id1, int id2) {
            Contact contact = contactsService.Get(id1);
            Message message = contact.messages.Find(x => x.Id == id2);
            contact.messages.Remove(message);
        }
    }
}
