using BookCatalog.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCatalog.DataAccess
{
    /// <summary>
    /// Represents the database context for the Book Catalog application.
    /// </summary>
    public class BookCatalogDbContext : DbContext
    {

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        public BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                // Title is required with a max length of 255
                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                // Author is required with a max length of 255
                entity.Property(b => b.Author)
                    .IsRequired()
                    .HasMaxLength(255);

                // ISBN is optional with a max length of 13
                entity.Property(b => b.ISBN)
                    .HasMaxLength(13);

                entity.Property(b => b.PublicationYear);
                entity.Property(b => b.Quantity);

                // Set up the foreign key for Category
                entity.HasOne(b => b.Category)
                    .WithMany(c => c.Books)
                    .HasForeignKey(b => b.CategoryId);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                // Name is required with a max length of 255
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                // Description is optional with a max length of 500
                entity.Property(c => c.Description)
                .HasMaxLength(500);
            });
        }
    }
}