using ex2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Services
{
    public interface IMessageService
    {
        public List<Message> GetAll();
        public Message Get(int id);
        public void Edit(Message message);
        public void Delete(int id);
        public void Add(string content, bool sent);
    }
}
