using Microsoft.EntityFrameworkCore;
using ContactsApi.Entities;

namespace ContactsApi.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
                base.OnModelCreating(modelBuilder);

                modelBuilder.HasAnnotation("Relational:HistoryTableName", "__EFMigrationsHistory");
                modelBuilder.HasAnnotation("Relational:HistoryTableSchema", "public");
                modelBuilder.HasDefaultSchema("public");
        }
}