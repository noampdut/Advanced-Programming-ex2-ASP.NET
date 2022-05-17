using ex2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Services
{
    public class ContactService : IContactService
    {
        private static List<Contact> contacts;
        public ContactService(List<Contact> contactsList)
        {
            contacts = contactsList;
        }
        public List<Contact> GetAll()
        {
            return contacts;
        }

        public Contact Get(string id)
        {
            return contacts.Find(x => x.Id == id);
        }

        public void Edit(Contact contact)
        {
            Contact temp = contacts.Find(x => x.Id == contact.Id);
            temp.LastDate = DateTime.Now.ToString();
            temp.LastMessage = contact.LastMessage;
        }
        public void Delete(string id)
        {
            Contact contact = Get(id);
            contacts.Remove(contact);
        }

        public void Add(string nickName, string id, string service)
        {
            Contact contact = new Contact() { Id = id, LastMessage = "", LastDate = "", NickName = nickName, Service = service, messages = new List<Message> { }, Picture = ""};
            contacts.Add(contact);
        }
    }
}
