using System.Diagnostics;

namespace BookCatalog.DataAccess.Models
{
    /// <summary>
/// Represents a category in the book catalog.
/// </summary>
[DebuggerDisplay("Id={Id}, Name={Name}")]
public class Category
{
    /// <summary>
    /// Gets or sets the unique identifier of the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the collection of books associated with this category.
    /// </summary>
    public virtual ICollection<Book> Books { get; set; }
}
}
