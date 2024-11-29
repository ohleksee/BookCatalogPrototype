using BookCatalog.DataAccess.Models;

namespace BookCatalog.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Defines methods for interacting with Category data in the database.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="category">The category to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddCategoryAsync(Category category);

        /// <summary>
        /// Deletes a category from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteCategoryAsync(int id);

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, returning a list of categories.</returns>
        Task<List<Category>> GetAllCategoriesAsync();

        /// <summary>
        /// Retrieves a category from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation, returning the category or null if not found.</returns>
        Task<Category?> GetCategoryByIdAsync(int id);

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="category">The updated category.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateCategoryAsync(Category category);
    }
}