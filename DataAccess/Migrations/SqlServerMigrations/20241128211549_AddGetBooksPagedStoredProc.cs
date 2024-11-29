using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookCatalog.DataAccess.Migrations
{
    /// <summary>
    /// Migration that creates the <c>GetBooksPaged</c> stored procedure in the database.
    /// This procedure supports paging, sorting, and searching books in the <c>Books</c> table.
    /// </summary>
    /// <remarks>
    /// The <c>GetBooksPaged</c> stored procedure accepts parameters for pagination, search, 
    /// and sorting, and retrieves a subset of books that match the search term. 
    /// The procedure allows sorting by specific columns and ensures that invalid columns 
    /// or sort directions default to predefined values.
    /// </remarks>
    public partial class AddGetBooksPagedStoredProc : Migration
    {
        /// <summary>
        /// Creates the <c>GetBooksPaged</c> stored procedure in the database.
        /// </summary>
        /// <param name="migrationBuilder">The builder to configure migration operations.</param>
        /// <remarks>
        /// This method creates the <c>GetBooksPaged</c> stored procedure, which can be used to retrieve a 
        /// paginated list of books based on the given parameters:
        /// - PageNumber: the page of results to retrieve.
        /// - PageSize: the number of records per page.
        /// - SearchTerm: the term to search for in the book's title or author.
        /// - SortColumn: the column to sort by (e.g., Title, Author, ISBN, PublicationYear).
        /// - SortDirection: the direction to sort (either 'ASC' for ascending or 'DESC' for descending).
        /// If the parameters for <c>SortColumn</c> or <c>SortDirection</c> are invalid, default values are applied.
        /// </remarks>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE GetBooksPaged
    @PageNumber INT,
    @PageSize INT,
    @SearchTerm NVARCHAR(255),
    @SortColumn NVARCHAR(50),
    @SortDirection NVARCHAR(4)
AS
BEGIN
    DECLARE @Offset INT;
    SET @Offset = (@PageNumber - 1) * @PageSize;

    -- Validate @SortColumn and @SortDirection
    IF @SortColumn NOT IN ('Title', 'Author', 'ISBN', 'PublicationYear')
    BEGIN
		-- Default to 'Title' if invalid
        SET @SortColumn = 'Title';
    END

    IF @SortDirection NOT IN ('ASC', 'DESC')
    BEGIN
		-- Default to 'ASC' if invalid
        SET @SortDirection = 'ASC'; 
    END

    -- Build the dynamic SQL query to include the column to order by and the direction
    DECLARE @DynamicSQL NVARCHAR(MAX);
    SET @DynamicSQL = N'
        SELECT * FROM Books
        WHERE Title LIKE @SearchTerm OR Author LIKE @SearchTerm
        ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + @SortDirection + '
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;';

    -- Execute the dynamic SQL with parameters
    EXEC sp_executesql @DynamicSQL, 
                       N'@SearchTerm NVARCHAR(255), @Offset INT, @PageSize INT', 
                       @SearchTerm, @Offset, @PageSize;
END;");
        }

        /// <summary>
        /// Drops the <c>GetBooksPaged</c> stored procedure from the database.
        /// </summary>
        /// <param name="migrationBuilder">The builder to configure migration operations for rollback.</param>
        /// <remarks>
        /// This method drops the <c>GetBooksPaged</c> stored procedure if it exists, 
        /// rolling back the changes made in the <see cref="Up"/> method.
        /// </remarks>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBooksPaged;");
        }
    }
}
