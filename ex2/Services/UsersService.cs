using ex2.Models;
using System.Collections.Generic;

namespace ex2.Services
{
    public class UsersService : IUsersService
    {
        private static List<User> usersList = new List<User> {};
        private static User acticeUser;

        public UsersService()
        {
            List<Contact> contactList = new List<Contact> { };
            Message message = new Message() { id = 1, created = "today", sent = true, content = "yesss" };
            List<Message> messages = new List<Message>() { message };
            contactList.Add(new Contact { id = "Lilach", lastDate = "today", last = "by", name = "lilach", messages = messages, server = "fds"});
            contactList.Add(new Contact { id = "OfekForn", lastDate = "today", last = "by", name = "ofek", messages = messages, server = "fds"});
            
            User user = new User() { Id = "noampdut", NickName = "Noamit", Password = "n123456", Contacts = contactList };

            usersList.Add(user);
            acticeUser = user; 
        }

        public List<User> GetAll()
        {
            return usersList;
        }
        public User GetActiveUser()
        {
            return acticeUser;
        }
        public bool isRegistered(string userName, string pwd)
        {
            if (usersList == null)
            {
                return false;
            } else
            {
                foreach(var user in usersList)
                {
                    if(user.Id == userName && user.Password == pwd)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool isSigned(string userName)
        {
            if (usersList == null)
            {
                return false;
            }
            else
            {
                foreach (var user in usersList)
                {
                    if (user.Id == userName)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public void Add(string id, string nickName, string pwd, string service)
        {
            User user = new User() { Id = id, Password = pwd, NickName = nickName, Contacts = new List<Contact> { } };
            usersList.Add(user);
        }

        public void setActiveUser(string userName)
        {
            foreach(var user in usersList)
            {
                if (user.Id == userName)
                {
                    acticeUser = user;
                    return;
                }
            }
        }
    }
}
