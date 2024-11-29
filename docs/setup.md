# Instructions for Running the Application

## 1. Prepare the Database

- **(Optional)** In the **DataAccess** project, update the `appsettings.Development.json` file to specify the connection string for `"bookCatalog"`. This connection string should point to the appropriate database where the application data will be stored. Default is targeting the *mssqllocaldb* on local machine with database *BookCatalog*

- **(Optional)** In the **DataAccess** project, update the `SeedTestData` value in `appsettings.Development.json` to indicate whether test data should be populated via database migration. The default value is `true`, which will seed the test data. Be aware that without test data, you will not be able to add books, as there is no API for category creation implemented. 

- In **Developer Powershell** in Visual Studio, execute the following command to apply the migrations and prepare the database: ``` dotnet ef database update -p DataAccess ```. This will update the database schema and, if `SeedTestData` is enabled, populate the database with test data.

## 2. Run the WebAPI Service

- **(Optional)** In the **WebAPI** project, update the `appsettings.json` file to specify the connection string for `"bookCatalog"`, pointing it to the database you set up in step 1.

- Open the solution in **Visual Studio** and run the **WebAPI** project.
  - In Visual Studio, go to the Run menu and select Start Without Debugging (or make the project as the startup and press Ctrl + F5).

- Ensure that the WebAPI is running and accessible via HTTP (check the output window for the URL). If not, reconfigure it by setting it as the startup project and changing the default profile of the Run operation.

## 3. Run the UI.WebForms Project

- In the **UI.WebForms** project, ensure that the `web.config` file has the correct URL for the `BookCatalogServiceUrl` property, pointing to the WebAPI service started in step 2.

- Start the **UI.WebForms** project:
  - In Visual Studio, you can run the project in **Debug Mode** (F5) or without debugging (`Ctrl + F5`).
  - Ensure that the **UI.WebForms** project is able to make requests to the running WebAPI service (verify via browser or console logs that requests are successfully reaching the API).
