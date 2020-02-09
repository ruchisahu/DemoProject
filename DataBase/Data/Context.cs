using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<DataBase.Model.Bug> BugItem { get; set; }
    }
}
