using ex2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Services
{
    public class MessageService : IMessageService
    {
        private static List<Message> messages;

        public MessageService(List<Message> messagesList)
        {
            messages = messagesList;
        }
        public List<Message> GetAll()
        {
            return messages;
        }

        public Message Get(int id)
        {
            return messages.Find(x => x.Id == id);
        }

        public void Edit(Message message)
        {
            Message temp = messages.Find(x => x.Id == message.Id);
            temp.Date = DateTime.Now.ToString();
            temp.Content = message.Content;
        }
        public void Delete(int id)
        {
            Message message = Get(id);
            messages.Remove(message);
        }

        public void Add(string content, bool sent)
        {
            int nextId;
            if (messages.Count != 0)
            {
                nextId = messages.Max(x => x.Id) + 1;
            }
            else
            {
                nextId = 1;
            }
            Message message = new Message() { Id = nextId, Content = content, Date = DateTime.Now.ToString(), sent = sent };
            messages.Add(message);
        }
    }
}
