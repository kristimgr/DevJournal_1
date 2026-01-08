using DevJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace DevJournal.Data
{
    public class JournalDbContext : DbContext
    {
        public DbSet<JournalEntry> JournalEntries { get; set; }

        public JournalDbContext(DbContextOptions<JournalDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JournalEntry>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.Property(e => e.PrimaryMood)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SecondaryMood1)
                    .HasMaxLength(50);

                entity.Property(e => e.SecondaryMood2)
                    .HasMaxLength(50);

                // Removed Category, CreatedAt, and UpdatedAt configurations
            });
        }
    }
}