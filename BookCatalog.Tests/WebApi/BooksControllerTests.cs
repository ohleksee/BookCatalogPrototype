using BookCatalog.DataAccess.Models;
using BookCatalog.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Contracts;
using Moq;
using WebAPI.Controllers;

namespace BookCatalog.Tests.WebApi
{
    /// <summary>
    /// Contains unit tests for the <see cref="BooksController"/> class.
    /// These tests verify the behavior of the controller actions such as retrieving, adding, updating, and deleting books.
    /// Mocks are used for the book and category repositories to simulate various scenarios without interacting with the database.
    /// </summary>
    [TestClass]
    public class BooksControllerTests
    {
        private Mock<IBookRepository> _mockBookRepository;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private BooksController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();

            _controller = new BooksController(_mockBookRepository.Object, _mockCategoryRepository.Object);
        }

        /// <summary>
        /// Tests that the GetBooks method returns an OkResult with a list of books when books are available.
        /// </summary>
        [TestMethod]
        public async Task GetBooks_ReturnsOkResult_WithBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book 1", Author = "Author 1", ISBN = "123456", PublicationYear = 2020, Quantity = 10, CategoryId = 1 },
                new Book { Id = 2, Title = "Test Book 2", Author = "Author 2", ISBN = "789012", PublicationYear = 2021, Quantity = 5, CategoryId = 2 }
            };

            _mockBookRepository.Setup(repo => repo.GetBooksPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                               .ReturnsAsync(books);

            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
                                   .ReturnsAsync(new Category { Id = 1, Name = "Fiction" });

            var result = await _controller.GetBooks(1, 100, "", "Title", "asc");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null.");
            Assert.AreEqual(200, okResult.StatusCode, "Expected HTTP status code 200 but got a different status code.");

            var bookDtos = okResult.Value as List<BookDto>;
            Assert.IsNotNull(bookDtos, "Expected a list of BookDto but got null.");
            Assert.AreEqual(2, bookDtos.Count, "Expected 2 BookDto objects but got a different count.");
            Assert.AreEqual("Fiction", bookDtos[0].CategoryName, "Expected category name 'Fiction' but got a different value.");
        }

        /// <summary>
        /// Tests that the GetBookById method returns a NotFoundResult when the requested book does not exist.
        /// </summary>
        [TestMethod]
        public async Task GetBookById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookId = 1;

            _mockBookRepository.Setup(repo => repo.GetBookByIdAsync(bookId))
                               .ReturnsAsync((Book?)null);

            var result = await _controller.GetBookById(bookId);

            Assert.IsInstanceOfType<NotFoundResult>(result, "Expected NotFoundResult but got a different result.");
        }

        /// <summary>
        /// Tests that the AddBook method returns an OkResult when a valid BookDto is provided.
        /// </summary>
        [TestMethod]
        public async Task AddBook_ReturnsOk_WhenValidBookDto()
        {
            var newBookDto = new BookDto
            {
                Title = "New Book",
                Author = "New Author",
                ISBN = "123123123",
                PublicationYear = 2024,
                Quantity = 5,
                CategoryId = 1
            };

            // Setup mock repository to simulate adding a book
            _mockBookRepository.Setup(repo => repo.AddBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            var result = await _controller.AddBook(newBookDto);

            var okResult = result as OkResult;
            Assert.IsNotNull(okResult, "Expected OkResult but got null.");
            Assert.AreEqual(200, okResult.StatusCode, "Expected HTTP status code 200 but got a different status code.");

            // Verify that AddBookAsync was called exactly once with the correct Book object
            _mockBookRepository.Verify(repo => repo.AddBookAsync(It.IsAny<Book>()), Times.Once, "Expected AddBookAsync to be called exactly once but it was not.");
        }

        /// <summary>
        /// Tests that the UpdateBook method returns a BadRequestResult when the ID in the URL does not match the ID in the BookDto.
        /// </summary>
        [TestMethod]
        public async Task UpdateBook_ReturnsBadRequest_WhenIdMismatch()
        {
            var bookDto = new BookDto
            {
                // This should not match the ID in the URL
                Id = 2,
                Title = "Updated Book",
                Author = "Updated Author",
                ISBN = "987654321",
                PublicationYear = 2025,
                Quantity = 10,
                CategoryId = 1
            };

            // Call the UpdateBook method with a different ID (e.g., URL ID is 1)
            var result = await _controller.UpdateBook(1, bookDto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult, "Expected BadRequest but got null.");
            Assert.AreEqual(400, badRequestResult.StatusCode, "Expected HTTP status code 400 but got a different status code.");
        }

        /// <summary>
        /// Tests that the UpdateBook method returns a NotFoundResult when the book to be updated does not exist.
        /// </summary>
        [TestMethod]
        public async Task UpdateBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookDto = new BookDto
            {
                Id = 1,
                Title = "Updated Book",
                Author = "Updated Author",
                ISBN = "987654321",
                PublicationYear = 2025,
                Quantity = 10,
                CategoryId = 1
            };

            // Setup mock repository to return null (indicating the book was not found)
            _mockBookRepository.Setup(repo => repo.GetBookByIdAsync(1)).ReturnsAsync((Book?)null);

            var result = await _controller.UpdateBook(1, bookDto);

            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult, "Expected NotFound but got null.");
            Assert.AreEqual(404, notFoundResult.StatusCode, "Expected HTTP status code 404 but got a different status code.");
        }

        /// <summary>
        /// Tests that the UpdateBook method returns an OkResult when the book is successfully updated.
        /// </summary>
        [TestMethod]
        public async Task UpdateBook_ReturnsOk_WhenBookUpdatedSuccessfully()
        {
            var bookDto = new BookDto
            {
                Id = 1,
                Title = "Updated Book",
                Author = "Updated Author",
                ISBN = "987654321",
                PublicationYear = 2025,
                Quantity = 10,
                CategoryId = 1
            };

            var existingBook = new Book
            {
                Id = 1,
                Title = "Old Book",
                Author = "Old Author",
                ISBN = "123456789",
                PublicationYear = 2020,
                Quantity = 5,
                CategoryId = 1
            };

            _mockBookRepository.Setup(repo => repo.GetBookByIdAsync(1)).ReturnsAsync(existingBook);
            _mockBookRepository.Setup(repo => repo.UpdateBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            var result = await _controller.UpdateBook(1, bookDto);

            var okResult = result as OkResult;
            Assert.IsNotNull(okResult, "Expected OkResult but got null.");
            Assert.AreEqual(200, okResult.StatusCode, "Expected HTTP status code 200 but got a different status code.");

            // Verify that UpdateBookAsync was called exactly once
            _mockBookRepository.Verify(repo => repo.UpdateBookAsync(It.IsAny<Book>()), Times.Once, "Expected UpdateBookAsync to be called exactly once but it was not.");

            // Verify the properties of the updated book
            Assert.AreEqual("Updated Book", existingBook.Title, "Expected book title to be 'Updated Book' but got a different value.");
            Assert.AreEqual("Updated Author", existingBook.Author, "Expected book author to be 'Updated Author' but got a different value.");
            Assert.AreEqual("987654321", existingBook.ISBN, "Expected book ISBN to be '987654321' but got a different value.");
            Assert.AreEqual(2025, existingBook.PublicationYear, "Expected publication year to be 2025 but got a different value.");
            Assert.AreEqual(10, existingBook.Quantity, "Expected book quantity to be 10 but got a different value.");
            Assert.AreEqual(1, existingBook.CategoryId, "Expected category ID to be 1 but got a different value.");
        }

        /// <summary>
        /// Tests that the DeleteBook method returns a NotFoundResult when the requested book does not exist.
        /// </summary>
        [TestMethod]
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookId = 1;
            // Simulate not found
            _mockBookRepository.Setup(repo => repo.GetBookByIdAsync(bookId))
                               .ReturnsAsync((Book?)null);

            var result = await _controller.DeleteBook(bookId);

            Assert.IsInstanceOfType<NotFoundResult>(result, "Expected NotFoundResult but got a different result.");
        }
    }
}