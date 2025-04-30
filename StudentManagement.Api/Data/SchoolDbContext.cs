using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Models;

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
            base.OnModelCreating(modelBuilder);

            // Unikátní přihlašovací jméno
            modelBuilder.Entity<Uzivatel>()
                .HasIndex(u => u.UzivatelskeJmeno)
                .IsUnique();

            // Oprava vztahu Znamka -> Student
            modelBuilder.Entity<Znamka>()
                .HasOne(z => z.Student)
                .WithMany()
                .HasForeignKey(z => z.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Oprava vztahu Znamka -> Predmet
            modelBuilder.Entity<Znamka>()
                .HasOne(z => z.Predmet)
                .WithMany()
                .HasForeignKey(z => z.PredmetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student -> Trida
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Trida)
                .WithMany()
                .HasForeignKey(s => s.TridaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student -> Rocnik
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Rocnik)
                .WithMany()
                .HasForeignKey(s => s.RocnikId)
                .OnDelete(DeleteBehavior.Restrict);

            // Predmet -> Rocnik
            modelBuilder.Entity<Predmet>()
                .HasOne(p => p.Rocnik)
                .WithMany()
                .HasForeignKey(p => p.RocnikId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
