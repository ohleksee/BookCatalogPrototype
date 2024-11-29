using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace BookCatalog.DataAccess.Migrations
{
    /// <summary>
    /// Migration that conditionally inserts seed data into the database based on configuration settings.
    /// This migration is used to seed test data into the database when running in a development environment.
    /// </summary>
    /// <remarks>
    /// This migration checks a configuration setting (from <c>appsettings.Development.json</c>) 
    /// to determine whether to insert seed data into the <c>Books</c> and <c>Categories</c> tables.
    /// It is specifically designed to seed test data when working with EF Core migrations in a development environment.
    /// The data inserted by this migration is for testing purposes and is not intended for production environments.
    /// </remarks>
    public partial class SeedTestDataConditionalMigration : Migration
    {
        /// <summary>
        /// Applies the migration to the database, conditionally inserting seed data if the configuration allows it.
        /// </summary>
        /// <param name="migrationBuilder">The builder to configure the migration operations.</param>
        /// <remarks>
        /// This method inserts predefined data into the <c>Categories</c> and <c>Books</c> tables
        /// only if the <c>SeedTestData</c> setting in the configuration is set to <c>true</c>.
        /// The seed data is intended for use in development or testing environments.
        /// </remarks>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            if (GetSeedTestDataOptionFromConfig())
            {
                migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction books including classics, novels, and stories.", "Fiction" },
                    { 2, "Non-fictional works including biographies, memoirs, and history.", "Non-Fiction" },
                    { 3, "Books on business strategies, leadership, and entrepreneurship.", "Business" },
                    { 4, "Books set in futuristic or scientific settings, including space exploration.", "Science Fiction" },
                    { 5, "Books detailing the lives and experiences of individuals.", "Biography" },
                    { 6, "Books on world history, political events, and historical figures.", "History" }
                });

                migrationBuilder.InsertData(
                    table: "Books",
                    columns: new[] { "Id", "Author", "CategoryId", "ISBN", "PublicationYear", "Quantity", "Title" },
                    values: new object[,]
                    {
                    { 1, "George Orwell", 1, "9780451524935", 1949, 15, "1984" },
                    { 2, "Harper Lee", 1, "9780061120084", 1960, 12, "To Kill a Mockingbird" },
                    { 3, "J.D. Salinger", 1, "9780316769488", 1951, 10, "The Catcher in the Rye" },
                    { 4, "Jane Austen", 1, "9780141439518", 1813, 14, "Pride and Prejudice" },
                    { 5, "Aldous Huxley", 1, "9780060850524", 1932, 8, "Brave New World" },
                    { 6, "F. Scott Fitzgerald", 1, "9780743273565", 1925, 9, "The Great Gatsby" },
                    { 7, "Yuval Noah Harari", 2, "9780062316110", 2011, 20, "Sapiens: A Brief History of Humankind" },
                    { 8, "Tara Westover", 2, "9780399590504", 2018, 7, "Educated: A Memoir" },
                    { 9, "Michelle Obama", 2, "9781524763138", 2018, 5, "Becoming" },
                    { 10, "Charles Duhigg", 2, "9781400069286", 2012, 10, "The Power of Habit" },
                    { 11, "Rebecca Skloot", 2, "9781400052189", 2010, 6, "The Immortal Life of Henrietta Lacks" },
                    { 12, "Sun Tzu", 2, "9781590302255", -500, 13, "The Art of War" },
                    { 13, "Eric Ries", 3, "9780307887894", 2011, 9, "The Lean Startup" },
                    { 14, "Benjamin Graham", 3, "9780060555665", 1949, 6, "The Intelligent Investor" },
                    { 15, "Jim Collins", 3, "9780066620992", 2001, 8, "Good to Great" },
                    { 16, "Simon Sinek", 3, "9781591846444", 2009, 7, "Start with Why" },
                    { 17, "Ben Horowitz", 3, "9780062273208", 2014, 5, "The Hard Thing About Hard Things" },
                    { 18, "Frank Herbert", 4, "9780441013593", 1965, 10, "Dune" },
                    { 19, "Orson Scott Card", 4, "9780812550702", 1985, 14, "Ender's Game" },
                    { 20, "Ursula K. Le Guin", 4, "9780441478125", 1969, 9, "The Left Hand of Darkness" },
                    { 21, "William Gibson", 4, "9780441569595", 1984, 5, "Neuromancer" },
                    { 22, "Isaac Asimov", 4, "9780553293357", 1951, 12, "Foundation" },
                    { 23, "Walter Isaacson", 5, "9781451648539", 2011, 6, "Steve Jobs" },
                    { 24, "Anne Frank", 5, "9780553296983", 1947, 15, "The Diary of a Young Girl" },
                    { 25, "Nelson Mandela", 5, "9780316548182", 1994, 8, "Long Walk to Freedom" },
                    { 26, "Susan Wise Bauer", 6, "9780393059748", 2007, 7, "The History of the Ancient World" },
                    { 27, "Barbara Tuchman", 6, "9780345476095", 1962, 10, "The Guns of August" },
                    { 28, "Howard Zinn", 6, "9780060838654", 1980, 13, "A People's History of the United States" },
                    { 29, "J.R.R. Tolkien", 1, "9780345339683", 1937, 18, "The Hobbit" },
                    { 30, "J.R.R. Tolkien", 1, "9780544003415", 1954, 12, "The Lord of the Rings" }
                    });
            }
        }

        /// <summary>
        /// Rolls back the migration by deleting seed data from the <c>Books</c> table if the configuration allows it.
        /// </summary>
        /// <param name="migrationBuilder">The builder to configure the migration operations for rollback.</param>
        /// <remarks>
        /// This method removes the seed data inserted by the <see cref="Up"/> method, but only if the configuration allows it.
        /// It is primarily used when rolling back a migration during development or testing.
        /// </remarks>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (GetSeedTestDataOptionFromConfig())
            {
                migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 2);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 3);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 4);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 5);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 6);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 7);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 8);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 9);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 10);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 11);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 12);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 13);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 14);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 15);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 16);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 17);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 18);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 19);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 20);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 21);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 22);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 23);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 24);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 25);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 26);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 27);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 28);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 29);

                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: 30);

                migrationBuilder.DeleteData(
                    table: "Categories",
                    keyColumn: "Id",
                    keyValue: 1);

                migrationBuilder.DeleteData(
                    table: "Categories",
                    keyColumn: "Id",
                    keyValue: 2);

                migrationBuilder.DeleteData(
                    table: "Categories",
                    keyColumn: "Id",
                    keyValue: 3);

                migrationBuilder.DeleteData(
                    table: "Categories",
                    keyColumn: "Id",
                    keyValue: 4);

                migrationBuilder.DeleteData(
                    table: "Categories",
                    keyColumn: "Id",
                    keyValue: 5);

                migrationBuilder.DeleteData(
                    table: "Categories",
                    keyColumn: "Id",
                    keyValue: 6);
            }
        }

        /// <summary>
        /// Retrieves the configuration setting for seed test data.
        /// </summary>
        /// <returns><c>true</c> if seed test data should be inserted; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method checks the <c>SeedTestData</c> configuration setting in <c>appsettings.Development.json</c>.
        /// If the value is <c>true</c>, the migration will insert seed data; otherwise, no data will be inserted.
        /// </remarks>
        private static bool GetSeedTestDataOptionFromConfig()
        {
            var configuration = ConfigHelper.GetDevelopmentConfiguration();
            var seedTestDataString = configuration?.GetSection("SeedTestData")?.Value;
            return bool.TryParse(seedTestDataString, out var seedTestData) && seedTestData;
        }
    }
}
