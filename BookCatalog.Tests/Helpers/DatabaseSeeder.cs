using BookCatalog.DataAccess;
using BookCatalog.DataAccess.Models;

namespace BookCatalog.Tests.Helpers
{
    internal static class DatabaseSeeder
    {
        /// <summary>
        /// Seeds the database with initial test data, including categories and books. 
        /// This method is typically used to populate an empty database or an in-memory 
        /// database with predefined records to enable consistent testing or application functionality.
        /// </summary>
        /// <param name="dbContext">The <see cref="BookCatalogDbContext"/> instance used to interact with the database.</param>
        /// <remarks>
        /// This method checks if the `Categories` and `Books` tables are empty, and if so, adds predefined 
        /// categories ("Fiction" and "Non-Fiction") and books (total 4) to the database. If the tables already contain data, it does not modify them.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="dbContext"/> is null.
        /// </exception>
        public static void Seed(BookCatalogDbContext dbContext)
        {
            // Seed categories if they don't exist
            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.Add(new Category { Name = "Fiction", Description = "Fiction books" });
                dbContext.Categories.Add(new Category { Name = "Non-Fiction", Description = "Non-Fiction books" });
                dbContext.SaveChanges();
            }

            // Seed books if they don't exist
            if (!dbContext.Books.Any())
            {
                dbContext.Books.Add(new Book
                {
                    Title = "The Shining",
                    Author = "Stephen King",
                    ISBN = "9780307743657",
                    PublicationYear = 1977,
                    Quantity = 10,
                    CategoryId = dbContext.Categories.First(c => c.Name == "Fiction").Id
                });

                dbContext.Books.Add(new Book
                {
                    Title = "It",
                    Author = "Stephen King",
                    ISBN = "9781501142970",
                    PublicationYear = 1986,
                    Quantity = 5,
                    CategoryId = dbContext.Categories.First(c => c.Name == "Fiction").Id
                });

                dbContext.Books.Add(new Book
                {
                    Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                    Author = "Robert C. Martin",
                    ISBN = "9780132350884",
                    PublicationYear = 2008,
                    Quantity = 8,
                    CategoryId = dbContext.Categories.First(c => c.Name == "Non-Fiction").Id
                });

                dbContext.Books.Add(new Book
                {
                    Title = "The Pragmatic Programmer: Your Journey to Mastery",
                    Author = "Andrew Hunt, David Thomas",
                    ISBN = "9780201616224",
                    PublicationYear = 1999,
                    Quantity = 6,
                    CategoryId = dbContext.Categories.First(c => c.Name == "Non-Fiction").Id
                });

                dbContext.SaveChanges();
            }
        }
    }
}
