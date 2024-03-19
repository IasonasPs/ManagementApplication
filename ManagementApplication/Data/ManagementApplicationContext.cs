using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ManagementApplication.Models;

namespace ManagementApplication.Data
{
    public class ManagementApplicationContext : DbContext
    {
        public ManagementApplicationContext (DbContextOptions<ManagementApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<ManagementApplication.Models.Candidate> Candidate { get; set; } = default!;
        public DbSet<ManagementApplication.Models.Degree> Degree { get; set; } = default!;
    }
}
