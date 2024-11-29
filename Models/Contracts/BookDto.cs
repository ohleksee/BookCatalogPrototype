namespace Models.Contracts
{
    /// <summary>
    /// Represents a book with its details for transfering in contracts.
    /// </summary>
    public class BookDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the book.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author of the book.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the International Standard Book Number (ISBN) of the book.
        /// </summary>
        public string ISBN { get; set; }

        /// <summary>
        /// Gets or sets the year of publication of the book.
        /// </summary>
        public int PublicationYear { get; set; }

        /// <summary>
        /// Gets or sets the quantity of available copies of the book.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the category the book belongs to.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category the book belongs to.
        /// </summary>
        public string CategoryName { get; set; }
    }
}
