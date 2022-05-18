using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Models
{
    public class Message
    {
        public int id { get; set; }
        public string content { get; set; }
        public string created { get; set; }
        public bool sent { get; set; }
    }
}
