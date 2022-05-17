using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ex2.Models;

namespace ex2.Data
{
    public class ex2Context : DbContext
    {
        public ex2Context (DbContextOptions<ex2Context> options)
            : base(options)
        {
        }

        public DbSet<ex2.Models.Rate> Rate { get; set; }
    }
}
