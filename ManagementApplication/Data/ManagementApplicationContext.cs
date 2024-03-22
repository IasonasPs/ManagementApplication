using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ManagementApplication.Models;
using System.ComponentModel;

namespace ManagementApplication.Data
{
    public class ManagementApplicationContext:DbContext
    {
        public ManagementApplicationContext(DbContextOptions<ManagementApplicationContext> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseInMemoryDatabase(databaseName : "ManamementApplicationDatabase");
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CandidateDegree>().HasKey(cd => new { cd.CandidateId, cd.DegreeId });

            modelBuilder.Entity<CandidateDegree>()
                                                .HasOne(cd => cd.Candidate)
                                                .WithMany(c => c.Degrees)
                                                .HasForeignKey(cd => cd.CandidateId);

            modelBuilder.Entity<CandidateDegree>()
                                                .HasOne(cd => cd.Degree)
                                                .WithMany(d => d.Candidates)
                                                .HasForeignKey(cd => cd.DegreeId);
        }


        public DbSet<Candidate> Candidate { get; set; } = default!;
        public DbSet<Degree> Degree { get; set; } = default!;
        public DbSet<CandidateDegree> CandidateDegree { get; set; }

    }
}
