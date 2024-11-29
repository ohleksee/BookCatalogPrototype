using BookCatalog.DataAccess;
using BookCatalog.DataAccess.Models;
using BookCatalog.DataAccess.Repositories;
using BookCatalog.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Tests.Repositories
{
    /// <summary>
    /// Contains unit tests for the <see cref="BookRepository"/> class.
    /// These tests validate the functionality of the repository methods for managing books in the database.
    /// The tests cover the core operations, including retrieving, adding, updating, and deleting books asynchronously.
    /// An in-memory database is used to isolate the tests and avoid any impact on the production environment.
    /// The <see cref="DatabaseSeeder"/> is used to seed initial data for testing purposes.
    /// </summary>
    [TestClass]
    public class BookRepositoryTests
    {
        private BookCatalogDbContext _dbContext;
        private BookRepository _bookRepository;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BookCatalogDbContext>()
                .UseInMemoryDatabase(databaseName: "BookCatalogTestDb_" + Guid.NewGuid().ToString())
                .Options;

            _dbContext = new BookCatalogDbContext(options);
            _bookRepository = new BookRepository(_dbContext);

            DatabaseSeeder.Seed(_dbContext);
        }

        /// <summary>
        /// Tests if all books can be retrieved from the database asynchronously.
        /// </summary>
        [TestMethod]
        public async Task GetAllBooks_ShouldReturnAllBooksAsync()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            Assert.AreEqual(4, books.Count);
        }

        /// <summary>
        /// Tests if a book can be retrieved by its Id asynchronously.
        /// </summary>
        [TestMethod]
        public async Task GetBookById_ShouldReturnBookByIdAsync()
        {
            var book = await _bookRepository.GetBookByIdAsync(1); // assuming the Id of the first book is 1
            Assert.IsNotNull(book);
            Assert.AreEqual("The Shining", book.Title);
            Assert.AreEqual("Stephen King", book.Author);
        }

        /// <summary>
        /// Tests if a book is correctly added to the database asynchronously.
        /// </summary>
        [TestMethod]
        public async Task AddBook_ShouldAddBookToDatabaseAsync()
        {
            var newBook = new Book
            {
                Title = "A Time to Love and a Time to Die",
                Author = "Erich Maria Remarque",
                ISBN = "9780449213926",
                PublicationYear = 1954,
                Quantity = 10,
                CategoryId = _dbContext.Categories.First(c => c.Name == "Fiction").Id
            };

            await _bookRepository.AddBookAsync(newBook);

            var addedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == "9780449213926");

            Assert.IsNotNull(addedBook);
            Assert.AreEqual("A Time to Love and a Time to Die", addedBook.Title);
            Assert.AreEqual("Erich Maria Remarque", addedBook.Author);
            Assert.AreEqual("9780449213926", addedBook.ISBN);
            Assert.AreEqual(1954, addedBook.PublicationYear);
            Assert.AreEqual(10, addedBook.Quantity);
        }

        /// <summary>
        /// Tests if the book details are correctly updated in the database asynchronously.
        /// </summary>
        [TestMethod]
        public async Task UpdateBook_ShouldUpdateBookDetailsAsync()
        {
            var book = await _dbContext.Books.FirstAsync();
            book.Title = "Updated Book Title";

            await _bookRepository.UpdateBookAsync(book);

            var updatedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);

            Assert.IsNotNull(updatedBook);
            Assert.AreEqual("Updated Book Title", updatedBook.Title);
        }

        /// <summary>
        /// Tests if a book is successfully removed from the database asynchronously.
        /// </summary>
        [TestMethod]
        public async Task DeleteBook_ShouldRemoveBookFromDatabaseAsync()
        {
            var book = await _dbContext.Books.FirstAsync();
            await _bookRepository.DeleteBookAsync(book.Id);

            var deletedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);

            Assert.IsNull(deletedBook);
        }

        /// <summary>
        /// Cleans up the test context and disposes the database context after each test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Dispose();
        }
    }
}