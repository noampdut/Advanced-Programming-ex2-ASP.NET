using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Models
{
    public class Contact
    {
        //public int Id { set; get; }
        public string Id { set; get; }
        public string NickName { set; get; }
        public string LastMessage { set; get; }
        public string LastDate { set; get; }
        public string Picture { set; get; }
        public string Service { set; get; }
        public List<Message> messages { set; get; }
    }
}
