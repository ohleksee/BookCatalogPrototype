using BookCatalog.DataAccess;
using BookCatalog.DataAccess.Repositories;
using BookCatalog.Tests.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Transactions;

namespace BookCatalog.Tests.Repositories
{
    /// <summary>
    /// Contains unit tests for the stored procedure-based methods in the <see cref="BookRepository"/> class.
    /// These tests validate the behavior of the repository when using stored procedures for paging, sorting, filtering,
    /// and preventing SQL injection vulnerabilities in queries related to books.
    /// The tests ensure that the stored procedure behaves as expected when interacting with the database, including:
    /// paging, sorting by specific columns, and handling invalid input or potential SQL injection attacks.
    /// A real SQL Server database connection is used to execute the tests, and each test is isolated within a transaction scope 
    /// to ensure data integrity and rollback after tests complete.
    /// The <see cref="DatabaseSeeder"/> is used to seed initial data for testing purposes.
    /// </summary>
    [TestClass]
    public class BookRepositoryStoredProcTests
    {
        private BookCatalogDbContext _dbContext;
        private BookRepository _bookRepository;
        private TransactionScope _transactionScope;

        [TestInitialize]
        public void Initialize()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connectionString = configuration.GetConnectionString("BookCatalogDb");
            var options = new DbContextOptionsBuilder<BookCatalogDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _dbContext = new BookCatalogDbContext(options);
            _bookRepository = new BookRepository(_dbContext);

            // Start a new transaction scope for the test
            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);

            DatabaseSeeder.Seed(_dbContext);
        }

        /// <summary>
        /// Tests if the books can be correctly paged, sorted, and filtered by title.
        /// </summary>
        [TestMethod]
        public async Task GetBooksPaged_ShouldReturnPagedAndSortedBooks()
        {
            var searchTerm = "The";
            var pageNumber = 1;
            var pageSize = 2;
            var sortColumn = "Title";
            var sortDirection = "ASC";

            var books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize, searchTerm, sortColumn, sortDirection);

            Assert.IsNotNull(books, "The returned books list should not be null.");
            Assert.IsTrue(books.Count <= pageSize, "The number of books returned exceeds the requested page size.");

            // Check if books are sorted by title in ascending order
            for (int i = 0; i < books.Count - 1; i++)
            {
                Assert.IsTrue(books[i].Title.CompareTo(books[i + 1].Title) < 0, "Books should be sorted by Title in ascending order.");
            }
        }

        /// <summary>
        /// Tests if the stored procedure correctly returns paged results for books.
        /// </summary>
        [TestMethod]
        public async Task GetBooksPagedStoredProc_ShouldReturnCorrectPagedResults()
        {
            var books = await _bookRepository.GetBooksPagedAsync(1, 2, "The", "Title", "ASC");

            Assert.AreEqual(2, books.Count, "The number of books returned does not match the expected count.");
            Assert.IsTrue(books.First().Title.CompareTo(books.Last().Title) < 0, "Books are not sorted in ascending order by Title.");
        }

        [TestMethod]
        public async Task GetBooksPaged_ShouldUseDefaultSortColumn_WhenInvalidSortColumnIsProvided()
        {
            // Pass an invalid sort column
            var invalidSortColumn = "InvalidColumn";
            var sortDirection = "ASC";
            var pageNumber = 1;
            var pageSize = 2;
            var searchTerm = "King";

            var books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize, searchTerm, invalidSortColumn, sortDirection);

            // The result should be ordered by the default column 'Title'
            var orderedBooks = books.OrderBy(b => b.Title).ToList();
            for (int i = 0; i < books.Count; i++)
            {
                Assert.AreEqual(orderedBooks[i].Title, books[i].Title, "Books are not sorted by the default column 'Title'.");
            }
        }

        [TestMethod]
        public async Task GetBooksPaged_ShouldUseDefaultSortDirection_WhenInvalidSortDirectionIsProvided()
        {
            // Pass an invalid sort direction
            var sortColumn = "Title";
            var invalidSortDirection = "Up";
            var pageNumber = 1;
            var pageSize = 2;
            var searchTerm = "King";

            var books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize, searchTerm, sortColumn, invalidSortDirection);

            // The result should be ordered by the 'Title' column in ascending order
            var orderedBooks = books.OrderBy(b => b.Title).ToList();
            for (int i = 0; i < books.Count; i++)
            {
                Assert.AreEqual(orderedBooks[i].Title, books[i].Title, "Books are not sorted in ascending order by Title, even with an invalid sort direction.");
            }
        }

        [TestMethod]
        public async Task GetBooksPaged_ShouldPreventSQLInjection_WhenSearchTermContainsSQLInjection()
        {
            // Pass a search term that could be an SQL injection
            var searchTerm = "'; DROP TABLE Books;--";
            var sortColumn = "Title";
            var sortDirection = "ASC";
            var pageNumber = 1;
            var pageSize = 2;

            var books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize, searchTerm, sortColumn, sortDirection);
            try
            {
                Assert.IsTrue(_dbContext.Books.Any(), "The Books table should not be empty. SQL injection might have succeeded.");
            }
            catch (SqlException ex)
            {
                // Books table was removed by injection
                if (ex.Message.Contains("Invalid object name"))
                    Assert.Fail("Expected SQL injection to be prevented, but it wasn't.");
            }
            Assert.AreEqual(0, books.Count, "Books were returned despite the SQL injection attempt.");
        }

        [TestMethod]
        public async Task GetBooksPaged_ShouldPreventSQLInjection_WhenSortColumnContainsSQLInjection()
        {
            // Pass an invalid sort column that could be an SQL injection
            var searchTerm = "King";
            var sortColumn = "'; DROP TABLE Books;--";
            var sortDirection = "ASC";
            var pageNumber = 1;
            var pageSize = 2;

            var books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize, searchTerm, sortColumn, sortDirection);
            try
            {
                Assert.IsTrue(_dbContext.Books.Any(), "The Books table should not be empty. SQL injection might have succeeded.");
            }
            catch (SqlException ex)
            {
                // Books table was removed by injection
                if (ex.Message.Contains("Invalid object name"))
                    Assert.Fail("Expected SQL injection to be prevented, but it wasn't.");
            }
        }

        [TestMethod]
        public async Task GetBooksPaged_ShouldPreventSQLInjection_WhenSortDirectionContainsSQLInjection()
        {
            // Pass an invalid sort direction that could be an SQL injection
            var searchTerm = "King";
            var sortColumn = "Title";
            var sortDirection = "'; DROP TABLE Books;--";
            var pageNumber = 1;
            var pageSize = 2;

            var books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize, searchTerm, sortColumn, sortDirection);
            try
            {
                Assert.IsTrue(_dbContext.Books.Any(), "The Books table should not be empty. SQL injection might have succeeded.");
            }
            catch (SqlException ex)
            {
                // Books table was removed by injection
                if (ex.Message.Contains("Invalid object name"))
                    Assert.Fail("Expected SQL injection to be prevented, but it wasn't.");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transactionScope.Dispose();
            _dbContext.Dispose();
        }
    }
}