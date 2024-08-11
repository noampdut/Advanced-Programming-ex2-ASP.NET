using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Models
{
    public class User
    {
        //public int Id { set; get; }
        public string Id { set; get; }
        public string NickName { set; get; }
        public string Picture { set; get; }
        public string Password { set; get; }
        public List<Contact> Contacts { set; get; } = new List<Contact>();  
    }
}
