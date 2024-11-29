using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookCatalog.DataAccess.Migrations
{
    /// <summary>
    /// A factory class for creating instances of <see cref="BookCatalogDbContext"/> 
    /// during design-time, typically used with Entity Framework (EF) migrations and database updates.
    /// </summary>
    /// <remarks>
    /// This class is used by EF Core tools (e.g., during <c>dotnet ef migrations add</c> or <c>dotnet ef database update</c>) 
    /// to create a <see cref="BookCatalogDbContext"/> instance when the application is running in development mode.
    /// The <see cref="CreateDbContext"/> method configures the database connection using the connection string 
    /// from the <c>appsettings.Development.json</c> configuration file to ensure migrations are applied correctly in development.
    /// </remarks>
    public class BookCatalogDesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookCatalogDbContext>
    {
        /// <summary>
        /// Creates and configures an instance of the <see cref="BookCatalogDbContext"/> 
        /// using the development environment's configuration settings.
        /// This method is primarily used by EF Core tooling to provide the correct 
        /// database context during migrations and database update operations.
        /// </summary>
        /// <param name="args">Optional arguments passed to the method. This parameter is not used in this implementation.</param>
        /// <returns>
        /// An instance of <see cref="BookCatalogDbContext"/> with the appropriate configuration for the development environment.
        /// </returns>
        /// <remarks>
        /// This method retrieves the connection string from the <c>appsettings.Development.json</c> configuration file 
        /// and configures it to use SQL Server for the <see cref="BookCatalogDbContext"/>.
        /// It is used when EF Core tools need to create a <see cref="BookCatalogDbContext"/> instance 
        /// for tasks like applying migrations or creating the database in a development environment.
        /// </remarks>
        public BookCatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookCatalogDbContext>();
            // Retrieve the development configuration to get the connection string.
            var configuration = ConfigHelper.GetDevelopmentConfiguration();
            var connectionString = configuration.GetConnectionString("bookCatalog");
            // Configure the DbContext to use SQL Server with the connection string.
            optionsBuilder.UseSqlServer(connectionString);
            // Return the configured DbContext instance.
            return new BookCatalogDbContext(optionsBuilder.Options);
        }
    }
}
