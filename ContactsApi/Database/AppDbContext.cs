using Microsoft.EntityFrameworkCore;
using ContactsApi.Models;

namespace ContactsApi.Database;

public class AppDbContext : DbContext
{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Contact>(entity =>
                {
                        entity.ToTable("Contacts");

                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.FirstName)
                        .IsRequired()
                        .HasMaxLength(50);

                        entity.Property(e => e.LastName)
                        .IsRequired()
                        .HasMaxLength(50);

                        entity.Property(e => e.Email)
                        .IsRequired()
                        .HasMaxLength(100);

                        entity.Property(e => e.PhoneNumber)
                        .HasMaxLength(20);

                        entity.Property(e => e.Address)
                        .HasMaxLength(200);

                        entity.Property(e => e.CreatedAt);

                        entity.Property(e => e.UpdatedAt);
                });
        }
}