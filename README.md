# Library's Book Inventory

## Task Overview
Develop a simple web application for managing a library's book inventory targeting the .NET Framework 4.8. The application should allow users to view, add, edit, and delete books.

## Requirements

### 1. Database Schema and Entities
Create a database schema using the Entity Framework Code First approach with the following entities:

- **Book**: 
  - Fields:
    - Title
    - Author
    - ISBN
    - Publication Year
    - Quantity
- **Category**:
  - Fields:
    - Category Name
    - Description

The **Book** entity should have a foreign key relationship with the **Category** entity.

### 2. Data Access Layer (CRUD Operations)
Implement a data access layer using Entity Framework Code First to perform CRUD (Create, Read, Update, Delete) operations on the **Book** and **Category** entities. 

Create a stored procedure for fetching books with:
- Pagination
- Full-text search
- Sorting (ascending and descending) by any of the columns

### 3. ASP.NET WebForms Application
Create an ASP.NET WebForms application with the following pages:

- **Home Page**: 
  - Display a list of books with their details (Title, Author, ISBN, Publication Year, Quantity) in a tabular format. No need for pagination here, just display all books.
  
- **Add Book Page**: 
  - A form to add a new book to the database. Include fields for:
    - Title
    - Author
    - ISBN
    - Publication Year
    - Quantity
    - A dropdown to select the book's category.
  
- **Edit Book Page**: 
  - A form to edit an existing book's details.

- **Delete Book Page**: 
  - A confirmation page to delete a book from the database.

### 4. ASP.NET Web API
Implement an ASP.NET Web API with the following endpoints:

- `GET api/books`: 
  - Returns a JSON response containing a list of all books with support for:
    - Pagination
    - Full-text search
    - Sorting
  
- `GET api/books/{id}`: 
  - Returns a JSON response containing the details of a specific book.
  
- `POST api/books`: 
  - Accepts JSON data to create a new book.
  
- `PUT api/books/{id}`: 
  - Accepts JSON data to update the details of a specific book.
  
- `DELETE api/books/{id}`: 
  - Deletes a specific book.

### 5. Unit Tests
Write unit tests using MSTest to test the functionality of:
- The data access layer
- API endpoints
- Any other critical components

### 6. Database
Use MS SQL Server as the database.

### 7. Instructions
Provide instructions on how to run the project.
