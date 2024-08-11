using ex2.Models;
using System;
using System.Collections.Generic;

namespace ex2.Services
{
    public class ContactService : IContactService
    {
        private static List<Contact> contacts;

        public ContactService(List<Contact> contactList)
        {
            contacts = contactList;
        }

        public List<Contact> GetAll()
        {

            return contacts;
        }

        public Contact Get(string id)
        {
            return contacts.Find(x => x.id == id);
        }

        public void Edit(Contact contact)
        {
            Contact temp = contacts.Find(x => x.id == contact.id);
            temp.lastDate = DateTime.Now.ToString();
            temp.last = contact.last;
        }
        public bool Delete(string id)
        {
            Contact contact = Get(id);
            if (contact != null)
            {
                contacts.Remove(contact);
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Add(string nickName, string id, string service, string picture)
        {
            Contact contact = new Contact() { id = id, last = "", lastDate = "", name = nickName, server = service, messages = new List<Message> { } , Picture = picture };
            contacts.Add(contact);
        }

    }
}
