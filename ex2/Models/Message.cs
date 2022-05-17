using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public bool sent { get; set; }
    }
}
