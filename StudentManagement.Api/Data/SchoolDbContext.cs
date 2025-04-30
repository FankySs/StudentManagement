using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StudentManagement.Api.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Studenti { get; set; }
        public DbSet<Trida> Tridy { get; set; }
        public DbSet<Rocnik> Rocniky { get; set; }
        public DbSet<Predmet> Predmety { get; set; }
        public DbSet<Znamka> Znamky { get; set; }
        public DbSet<Uzivatel> Uzivatele { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Uzivatel>()
                .HasIndex(u => u.UzivatelskeJmeno)
                .IsUnique();
        }
    }
}
