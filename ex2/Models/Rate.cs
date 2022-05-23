using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ex2.Models
{
    public class Rate
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Score must be between 1 to 5")]
        public int Score { get; set; }

        [Required]
        [StringLength(150)]
        public string Text { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
    }
}

