using Microsoft.Extensions.Configuration;

namespace BookCatalog.DataAccess.Migrations
{
    /// <summary>
    /// Provides helper methods for retrieving configuration settings specific to the environment.
    /// Primarily used during Entity Framework (EF) migration and database generation processes.
    /// </summary>   
    internal static class ConfigHelper
    {
        /// <summary>
        /// Retrieves the configuration settings specific to the development environment from the <c>appsettings.Development.json</c> file.
        /// This method is primarily used in scenarios such as Entity Framework migrations and database generation.
        /// </summary>
        /// <returns>
        /// An <see cref="IConfigurationRoot"/> object containing the configuration settings from the <c>appsettings.Development.json</c> file.
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if the <c>appsettings.Development.json</c> file is not found in the current directory.
        /// </exception>
        /// <remarks>
        /// This method assumes that the configuration file <c>appsettings.Development.json</c> exists in the current directory.
        /// If the file is not found, an exception is thrown to alert the user that the required configuration file is missing.
        /// This method is typically used during the development phase for tasks such as EF migrations.
        /// </remarks>
        public static IConfigurationRoot GetDevelopmentConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            return configuration ?? throw new Exception("appsettings.Development.json was not found!");
        }
    }
}
