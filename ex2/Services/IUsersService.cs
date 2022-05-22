using ex2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Services
{
    public interface IUsersService
    {
        public List<User> GetAll();
        public User GetActiveUser();
        public bool isRegistered(string userName, string pwd);
        public void Add(string id, string nickName, string pwd, string service);
        public void setActiveUser(string userName);
        public bool isSigned(string userName);
    }
}
