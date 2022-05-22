using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Models
{
    public class Contact
    {
        //public int Id { set; get; }
        public string id { set; get; }
        public string name { set; get; }
        public string server { set; get; }
        public string last { set; get; }
        public string lastDate { set; get; }
        //public string Picture { set; get; }
        public List<Message> messages { set; get; }
    }
}
