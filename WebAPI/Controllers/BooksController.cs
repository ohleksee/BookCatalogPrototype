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

        public BooksController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
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
            List<Book> books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize, searchTerm, sortColumn, sortDirection);
            var categoriesCache = new Dictionary<int, string>();
            var bookDtos = books.Select((Book book) =>
            {
                if (!categoriesCache.TryGetValue(book.CategoryId, out var categoryName))
                    categoryName = _categoryRepository.GetCategoryByIdAsync(book.CategoryId).Result?.Name;

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

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>A BookDto object representing the book if found, or NotFound if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            Book? book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();

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

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="bookDto">The BookDto object representing the book to add.</param>
        /// <returns>Ok if the book is added successfully, or BadRequest if the book data is not provided.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto bookDto)
        {
            if (id != bookDto.Id)
                return BadRequest("Book ID mismatch.");

            var existingBook = await _bookRepository.GetBookByIdAsync(id);
            if (existingBook == null)
                return NotFound();

            existingBook.Title = bookDto.Title;
            existingBook.Author = bookDto.Author;
            existingBook.ISBN = bookDto.ISBN;
            existingBook.PublicationYear = bookDto.PublicationYear;
            existingBook.Quantity = bookDto.Quantity;
            existingBook.CategoryId = bookDto.CategoryId;

            await _bookRepository.UpdateBookAsync(existingBook);
            return Ok();
        }

        /// <summary>
        /// Deletes a book from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>NoContent if the book is deleted successfully, or NotFound if the book is not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();

            await _bookRepository.DeleteBookAsync(id);
            return NoContent();
        }

#if DEBUG
        [HttpPost("addPredefined")]
        public async Task<IActionResult> AddBookPredefined()
        {
            var additionalBooks = new List<BookDto>
            {
                // Fiction Books
                new BookDto { Title = "Catch-22", Author = "Joseph Heller", ISBN = "9781451626650", PublicationYear = 1961, Quantity = 7, CategoryId = 59 },
                new BookDto { Title = "The Odyssey", Author = "Homer", ISBN = "9780143039952", PublicationYear = -800, Quantity = 9, CategoryId = 59 },
                new BookDto { Title = "Frankenstein", Author = "Mary Shelley", ISBN = "9780486282114", PublicationYear = 1818, Quantity = 6, CategoryId = 59 },
                new BookDto { Title = "The Picture of Dorian Gray", Author = "Oscar Wilde", ISBN = "9780141439570", PublicationYear = 1890, Quantity = 5, CategoryId = 59 },
                new BookDto { Title = "Les Misérables", Author = "Victor Hugo", ISBN = "9780140444308", PublicationYear = 1862, Quantity = 4, CategoryId = 59 },

                // Non-Fiction Books
                new BookDto { Title = "Astrophysics for People in a Hurry", Author = "Neil deGrasse Tyson", ISBN = "9780393609394", PublicationYear = 2017, Quantity = 8, CategoryId = 60 },
                new BookDto { Title = "Into the Wild", Author = "Jon Krakauer", ISBN = "9780385486804", PublicationYear = 1996, Quantity = 10, CategoryId = 60 },
                new BookDto { Title = "The Gene: An Intimate History", Author = "Siddhartha Mukherjee", ISBN = "9781476733531", PublicationYear = 2016, Quantity = 12, CategoryId = 60 },
                new BookDto { Title = "The Body: A Guide for Occupants", Author = "Bill Bryson", ISBN = "9780385539302", PublicationYear = 2019, Quantity = 6, CategoryId = 60 },
                new BookDto { Title = "The Power of Habit", Author = "Charles Duhigg", ISBN = "9781400069286", PublicationYear = 2012, Quantity = 9, CategoryId = 60 }
            };

            foreach (var bookDto in additionalBooks)
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
            }

            return Ok();
        }
#endif
    }
}
