using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Models
{
    public class FrontEndContext : DbContext
    {
        public FrontEndContext (DbContextOptions<FrontEndContext> options)
            : base(options)
        {
        }

        public DbSet<FrontEnd.Models.Bug> Bug { get; set; }
    }
}
