using System.Diagnostics;

namespace BookCatalog.DataAccess.Models
{
    /// <summary>
    /// Represents a book in the catalog with its details.
    /// </summary>
    [DebuggerDisplay("Id={Id}, Title={Title}")]
    public class Book
    {
        /// <summary>
        /// Gets or sets the unique identifier for the book.
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
        /// Gets or sets the publication year of the book.
        /// </summary>
        public int PublicationYear { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the book available in the catalog.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the category to which the book belongs.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category of the book.
        /// </summary>
        public virtual Category Category { get; set; }
    }
}
