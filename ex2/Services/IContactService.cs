using ex2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Services
{
    public interface IContactService
    {
        public List<Contact> GetAll();
        public Contact Get(string id);

        public void Edit(Contact contact);
        public bool Delete(string id);
        public void Add(string nickName, string id, string service);
    }
}
