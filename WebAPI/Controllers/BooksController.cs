using BookCatalog.DataAccess.Models;
using BookCatalog.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Contracts;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing books in the book catalog.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<BooksController> _logger;
        private readonly string _requestId;

        public BooksController(IBookRepository bookRepository, ICategoryRepository categoryRepository, ILogger<BooksController> logger,
                               IHttpContextAccessor httpContextAccessor)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
            _requestId = httpContextAccessor?.HttpContext?.Items["RequestId"]?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Retrieves a paginated list of books, with optional search and sorting.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of books per page (default is 100).</param>
        /// <param name="searchTerm">The search term to filter books by title or author (default is empty).</param>
        /// <param name="sortColumn">The column to sort by (default is "Title").</param>
        /// <param name="sortDirection">The sort direction (default is "asc").</param>
        /// <returns>A list of BookDto objects representing the books.</returns>
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100,
                                                  [FromQuery] string searchTerm = "", [FromQuery] string sortColumn = "Title",
                                                  [FromQuery] string sortDirection = "asc")
        {
            try
            {
                List<Book> books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize, searchTerm, sortColumn, sortDirection);
                var categoriesCache = new Dictionary<int, string>();
                var bookDtos = books.Select((Book book) =>
                {
                    if (!categoriesCache.TryGetValue(book.CategoryId, out var categoryName))
                    {
                        categoryName = _categoryRepository.GetCategoryByIdAsync(book.CategoryId).Result?.Name;
                        if (categoryName != null)
                            categoriesCache[book.CategoryId] = categoryName;
                    }

                    return new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        ISBN = book.ISBN,
                        PublicationYear = book.PublicationYear,
                        Quantity = book.Quantity,
                        CategoryId = book.CategoryId,
                        CategoryName = categoryName
                    };
                }).ToList();

                return Ok(bookDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving books.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>A BookDto object representing the book if found, or NotFound if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                Book? book = await _bookRepository.GetBookByIdAsync(id);
                if (book == null)
                {
                    _logger.LogWarning($"Request ID: {_requestId} - Book with ID {id} not found.");
                    return NotFound();
                }

                var bookDto = new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    ISBN = book.ISBN,
                    PublicationYear = book.PublicationYear,
                    Quantity = book.Quantity,
                    CategoryId = book.CategoryId,
                    CategoryName = book.Category.Name
                };

                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving book with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="bookDto">The BookDto object representing the book to add.</param>
        /// <returns>Ok if the book is added successfully, or BadRequest if the book data is not provided.</returns>
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            if (bookDto == null)
                return BadRequest("Book data is required.");
            try
            {
                var book = new Book
                {
                    Title = bookDto.Title,
                    Author = bookDto.Author,
                    ISBN = bookDto.ISBN,
                    PublicationYear = bookDto.PublicationYear,
                    Quantity = bookDto.Quantity,
                    CategoryId = bookDto.CategoryId
                };

                await _bookRepository.AddBookAsync(book);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding new book");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="bookDto">The BookDto object representing the book to add.</param>
        /// <returns>Ok if the book is added successfully, or BadRequest if the book data is not provided.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                _logger.LogWarning($"Request ID: {_requestId} - Book request id {id} is different from DTO id {bookDto.Id}.");
                return BadRequest("Book ID mismatch.");
            }
            try
            {
                var existingBook = await _bookRepository.GetBookByIdAsync(id);
                if (existingBook == null)
                {
                    _logger.LogWarning($"Request ID: {_requestId} - Book with ID {id} not found.");
                    return NotFound();
                }

                existingBook.Title = bookDto.Title;
                existingBook.Author = bookDto.Author;
                existingBook.ISBN = bookDto.ISBN;
                existingBook.PublicationYear = bookDto.PublicationYear;
                existingBook.Quantity = bookDto.Quantity;
                existingBook.CategoryId = bookDto.CategoryId;

                await _bookRepository.UpdateBookAsync(existingBook);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating book with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a book from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>NoContent if the book is deleted successfully, or NotFound if the book is not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);
                if (book == null)
                {
                    _logger.LogWarning($"Request ID: {_requestId} - Book with ID {id} not found.");
                    return NotFound();
                }

                await _bookRepository.DeleteBookAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting book with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
