using BookCatalog.DataAccess;
using BookCatalog.DataAccess.Models;
using BookCatalog.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Tests.Repositories
{
    /// <summary>
    /// Contains unit tests for the <see cref="CategoryRepository"/> class.
    /// These tests validate the functionality of the repository methods, including adding, updating, retrieving, and deleting categories.
    /// An in-memory database is used for testing to simulate interactions with a real database without affecting production data.
    /// The tests ensure the repository methods behave correctly under various conditions.
    /// </summary>
    [TestClass]
    public class CategoryRepositoryTests
    {
        private BookCatalogDbContext _dbContext;
        private CategoryRepository _categoryRepository;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BookCatalogDbContext>()
                .UseInMemoryDatabase(databaseName: "BookCatalogTestDb_" + Guid.NewGuid().ToString())
                .Options;

            _dbContext = new BookCatalogDbContext(options);
            _categoryRepository = new CategoryRepository(_dbContext);

            SeedDatabase();
        }

        /// <summary>
        /// Defines initial data for testing, adding sample categories to the database.
        /// </summary>
        private void SeedDatabase()
        {
            if (!_dbContext.Categories.Any())
            {
                _dbContext.Categories.Add(new Category { Name = "Fiction", Description = "Fiction books" });
                _dbContext.Categories.Add(new Category { Name = "Non-Fiction", Description = "Non-Fiction books" });
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Tests if all categories are correctly retrieved from the database.
        /// </summary>
        [TestMethod]
        public async Task GetAllCategories_ShouldReturnAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            Assert.AreEqual(2, categories.Count, "Expected 2 categories in the database, but the count was different.");
        }

        /// <summary>
        /// Tests if a category can be retrieved by its ID.
        /// </summary>
        [TestMethod]
        public async Task GetCategoryById_ShouldReturnCorrectCategoryAsync()
        {
            var category = _dbContext.Categories.First();

            var retrievedCategory = await _categoryRepository.GetCategoryByIdAsync(category.Id);

            Assert.IsNotNull(retrievedCategory, "Expected a category to be returned but got null.");
            Assert.AreEqual(category.Id, retrievedCategory.Id, "The retrieved category ID does not match the expected ID.");
            Assert.AreEqual(category.Name, retrievedCategory.Name, "The retrieved category name does not match the expected name.");
        }

        /// <summary>
        /// Tests if a new category can be successfully added to the database.
        /// </summary>
        [TestMethod]
        public async Task AddCategory_ShouldAddCategoryToDatabaseAsync()
        {
            var newCategory = new Category { Name = "Science", Description = "Science books" };

            await _categoryRepository.AddCategoryAsync(newCategory);

            var addedCategory = _dbContext.Categories.FirstOrDefault(c => c.Name == "Science");
            Assert.IsNotNull(addedCategory, "Expected 'Science' category to be added to the database but got null.");
            Assert.AreEqual("Science", addedCategory.Name, "The category name was not set correctly.");
            Assert.AreEqual("Science books", addedCategory.Description, "The category description was not set correctly.");
        }

        /// <summary>
        /// Tests if an existing category can be updated successfully.
        /// </summary>
        [TestMethod]
        public async Task UpdateCategory_ShouldUpdateCategoryDetailsAsync()
        {
            var category = _dbContext.Categories.First();
            category.Name = "Updated Category Name";
            category.Description = "Updated Description";
            await _categoryRepository.UpdateCategoryAsync(category);

            var updatedCategory = _dbContext.Categories.First(c => c.Id == category.Id);
            Assert.AreEqual("Updated Category Name", updatedCategory.Name, "The category name was not updated correctly.");
            Assert.AreEqual("Updated Description", updatedCategory.Description, "The category description was not updated correctly.");
        }

        /// <summary>
        /// Tests if a category can be deleted successfully.
        /// </summary>
        [TestMethod]
        public async Task DeleteCategory_ShouldRemoveCategoryFromDatabaseAsync()
        {
            var category = _dbContext.Categories.First();

            await _categoryRepository.DeleteCategoryAsync(category.Id);

            var deletedCategory = _dbContext.Categories.FirstOrDefault(c => c.Id == category.Id);
            Assert.IsNull(deletedCategory, "Expected category to be deleted but it was still found in the database.");
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