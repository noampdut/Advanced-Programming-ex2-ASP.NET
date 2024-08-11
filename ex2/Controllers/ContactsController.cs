using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ex2.Hubs;
using ex2.Models;
using ex2.Services;
using Microsoft.AspNetCore.Http;
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
            if (contactsList != null)
            {
                return Json(contactsList);
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
                var contact_as_user = userService.Get(id);
                string picture;
                if (contact_as_user != null)
                {
                    picture = contact_as_user.Picture;
                } else
                {
                    picture = string.Empty;
                }
                contactsService.Add(name, id, server, picture);
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
        /*
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
        */

        /*
                [HttpPost("{user}/{id}/Messages"), ActionName("Messages")]
                public async Task<IActionResult> createMessage(string user, string id, [FromForm] string content, [FromForm] IFormFile file, [FromForm] IFormFile audio)
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

                    string type = "";
                    // Handle file upload
                    string filePath = null;
                    if (file != null)
                    {
                        type = "img";
                        var fileName = Path.GetFileName(file.FileName);
                        var uploadDir = Path.Combine("uploads", "images");

                        // Ensure the directory exists
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        filePath = Path.Combine(uploadDir, fileName);
                        filePath = Path.GetFullPath(filePath); // Ensure the path is fully qualified

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Log or debug the file path to see where it is being saved
                        Console.WriteLine($"File saved to: {filePath}");
                    }


                    // Handle audio upload
                    string audioPath = null;
                    if (audio != null)
                    {
                        type = "audio";
                        var audioName = Path.GetFileName(audio.FileName);
                        audioPath = Path.Combine("uploads", "audio", audioName);

                        using (var stream = new FileStream(audioPath, FileMode.Create))
                        {
                            await audio.CopyToAsync(stream);
                        }
                    }

                    // Create the message
                    string date = DateTime.Now.ToString();
                    int nextId = contact.messages.Any() ? contact.messages.Max(x => x.id) + 1 : 1;
                    if (type == "")
                    {
                        type = "text";
                    }
                    Message message = new Message()
                    {
                        id = nextId,
                        content = content,
                        FilePath = filePath,
                        AudioPath = audioPath,
                        Created = date,
                        sent = false,
                        Type = type
                    };

                    contact.messages.Add(message);
                    contact.last = message.content;
                    contact.lastDate = message.Created;

                    await hubContext.Clients.All.SendAsync("getNewMessage");

                    return StatusCode(201);
                }
                */


        [HttpPost("{user}/{id}/Messages"), ActionName("Messages")]
        public async Task<IActionResult> createMessage(string user, string id, [FromForm] string content, [FromForm] IFormFile file, [FromForm] IFormFile audio)
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

            string type = "";
            // Handle file upload
            string filePath = null;
            if (file != null)
            {
                type = "img";
                var fileName = Path.GetFileName(file.FileName);
                var uploadDir = Path.Combine("wwwroot", "uploads", "images");

                // Ensure the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate a URL for the file
                filePath = $"/uploads/images/{fileName}";
            }

            // Handle audio upload
            string audioPath = null;
            if (audio != null)
            {
                type = "audio";
                var audioName = Path.GetFileName(audio.FileName);
                var audioDir = Path.Combine("wwwroot", "uploads", "audio");

                if (!Directory.Exists(audioDir))
                {
                    Directory.CreateDirectory(audioDir);
                }

                audioPath = Path.Combine(audioDir, audioName);

                using (var stream = new FileStream(audioPath, FileMode.Create))
                {
                    await audio.CopyToAsync(stream);
                }

                // Generate a URL for the audio file
                audioPath = $"/uploads/audio/{audioName}";
            }

            // Create the message
            string date = DateTime.Now.ToString();
            int nextId = contact.messages.Any() ? contact.messages.Max(x => x.id) + 1 : 1;
            if (type == "")
            {
                type = "text";
            }
            Message message = new Message()
            {
                id = nextId,
                content = content,
                FilePath = filePath,
                AudioPath = audioPath,
                Created = date,
                sent = false,
                Type = type
            };

            contact.messages.Add(message);
            contact.last = message.content;
            contact.lastDate = message.Created;

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
                    message.Created = DateTime.Now.ToString();
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
