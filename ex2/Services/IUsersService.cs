using ex2.Models;
using System.Collections.Generic;

namespace ex2.Services
{
    public interface IUsersService
    {
        public List<User> GetAll();
        //public User GetactiveUser();
        public bool isRegistered(string userName, string pwd);
        public void Add(string id, string nickName, string pwd, string service, string picture);
        //public void setActiveUser(string userName);
        public bool isSigned(string userName);
        public User Get(string id);
    }
}
