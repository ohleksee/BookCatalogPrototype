using BookCatalog.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Contracts;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing categories in the book catalog.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Retrieves all categories from the book catalog asynchronously.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the action.
        /// The result is a list of <see cref="CategoryDto"/> objects.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var bookDtos = categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            }).ToList();
            return Ok(bookDtos);
        }
    }
}