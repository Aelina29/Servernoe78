namespace UniversitiesMVC.Data;

using global::UniversitiesMVC.Models;
using Microsoft.EntityFrameworkCore;


    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<UniversityEntity> University { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityEntity>().ToTable("contacts");
        }
    }