using BookCatalog.DataAccess.Models;
using BookCatalog.DataAccess.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.DataAccess.Repositories
{
    /// <summary>
    /// Represents a repository for managing book data.
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private readonly BookCatalogDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context for book catalog operations.</param>
        public BookRepository(BookCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all books from the database, including their associated category.
        /// </summary>
        /// <returns>A list of books.</returns>
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _dbContext.Books.Include(b => b.Category).ToListAsync();
        }

        /// <summary>
        /// Retrieves a paged list of books from the database based on search criteria and sorting options.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of books per page.</param>
        /// <param name="searchTerm">The search term to filter books.</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <param name="sortDirection">The sorting direction (asc or desc).</param>
        /// <returns>A list of books that match the search criteria and are sorted according to the specified options.</returns>
        public async Task<List<Book>> GetBooksPagedAsync(int pageNumber, int pageSize, string searchTerm, string sortColumn, string sortDirection)
        {
            var searchParam = "%" + searchTerm + "%";
            return await _dbContext.Books.FromSqlRaw(
                "EXEC GetBooksPaged @PageNumber, @PageSize, @SearchTerm, @SortColumn, @SortDirection",
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@SearchTerm", searchParam),
                new SqlParameter("@SortColumn", sortColumn),
                new SqlParameter("@SortDirection", sortDirection)
            ).ToListAsync();
        }

        /// <summary>
        /// Retrieves a book by its unique identifier, including its associated category.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <returns>The book with the specified identifier, or null if not found.</returns>
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _dbContext.Books.Include(b => b.Category)
                                         .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">The book to add.</param>
        public async Task AddBookAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing book in the database.
        /// </summary>
        /// <param name="book">The updated book information.</param>
        public async Task UpdateBookAsync(Book book)
        {
            var existingBook = await _dbContext.Books.FindAsync(book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.PublicationYear = book.PublicationYear;
                existingBook.Quantity = book.Quantity;
                existingBook.CategoryId = book.CategoryId;

                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a book from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book to delete.</param>
        public async Task DeleteBookAsync(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
