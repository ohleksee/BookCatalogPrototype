using Models.Contracts;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UI.WebForms.ApiProxy
{
    /// <summary>
    /// A class responsible for making API calls related to categories.
    /// </summary>
    public class CategoryServiceCaller
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryServiceCaller"/> class.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance used for making API calls.</param>
    public CategoryServiceCaller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves a list of all categories from the API.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of CategoryDto objects.</returns>
    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("categories");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<List<CategoryDto>>();
    }
}
}