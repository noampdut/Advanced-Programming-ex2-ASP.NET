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
        public bool sent { get; set; }

        public string FilePath { get; set; }
        public string AudioPath { get; set; }
        public string Created { get; set; }

        public string Type { get; set; }

        // New properties for handling file uploads
        // public byte[] FileData { get; set; } // For storing file binary data
        //public string FileName { get; set; } // For storing file name
        //public string FileContentType { get; set; } // For storing file MIME type

    }
}
