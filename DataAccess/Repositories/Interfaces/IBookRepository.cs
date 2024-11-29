using BookCatalog.DataAccess.Models;

namespace BookCatalog.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Defines the contract for interacting with book data.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Adds a new book to the repository asynchronously.
        /// </summary>
        /// <param name="book">The book to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddBookAsync(Book book);

        /// <summary>
        /// Deletes a book from the repository asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the book to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteBookAsync(int id);

        /// <summary>
        /// Retrieves all books from the repository asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning a list of books.</returns>
        Task<List<Book>> GetAllBooksAsync();

        /// <summary>
        /// Retrieves a book by its unique identifier from the repository asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the book to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, returning the book or null if not found.</returns>
        Task<Book?> GetBookByIdAsync(int id);

        /// <summary>
        /// Retrieves a paged list of books from the repository asynchronously based on search and sorting criteria.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of books per page.</param>
        /// <param name="searchTerm">The search term to filter books.</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <param name="sortDirection">The sort direction (asc or desc).</param>
        /// <returns>A task representing the asynchronous operation, returning a list of books.</returns>
        Task<List<Book>> GetBooksPagedAsync(int pageNumber, int pageSize, string searchTerm, string sortColumn, string sortDirection);

        /// <summary>
        /// Updates an existing book in the repository asynchronously.
        /// </summary>
        /// <param name="book">The updated book.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateBookAsync(Book book);
    }
}