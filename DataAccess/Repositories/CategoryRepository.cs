using BookCatalog.DataAccess.Models;
using BookCatalog.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.DataAccess.Repositories
{
    /// <summary>
    /// Represents a repository for managing categories in the database.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookCatalogDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="BookCatalogDbContext"/> used to access the database.</param>
        public CategoryRepository(BookCatalogDbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all categories.</returns>
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the category if found; otherwise, null.</returns>
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="category">The category object to add.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task AddCategoryAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing category in the database with new values.
        /// </summary>
        /// <param name="category">The category object containing updated information.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a category from the database by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
