using Models.Contracts;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UI.WebForms.ApiProxy
{
    /// <summary>
    /// A class responsible for making API calls related to books.
    /// </summary>
    public class BookServiceCaller
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookServiceCaller"/> class.
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to be used for making API calls.</param>
        public BookServiceCaller(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves all books from the API.
        /// </summary>
        /// <returns>A list of BookDto objects representing the books.</returns>
        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            var response = await _httpClient.GetAsync("books");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<BookDto>>();
        }

        /// <summary>
        /// Retrieves a book by its ID from the API.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>A BookDto object representing the book.</returns>
        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"books/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<BookDto>();
        }

        /// <summary>
        /// Adds a new book to the API.
        /// </summary>
        /// <param name="book">The BookDto object representing the book to add.</param>
        public async Task AddBookAsync(BookDto book)
        {
            var response = await _httpClient.PostAsJsonAsync("books", book);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates an existing book in the API.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="book">The BookDto object representing the updated book.</param>
        public async Task UpdateBookAsync(int id, BookDto book)
        {
            var response = await _httpClient.PutAsJsonAsync($"books/{id}", book);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Deletes a book from the API by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        public async Task DeleteBookAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"books/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}