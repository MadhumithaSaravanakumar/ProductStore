# ProductStore

## Overview

ProductManagement is a modular .NET solution designed for scalable, maintainable, and testable enterprise applications. The project demonstrates:
- **Clean Architecture**: Separation of concerns across Common, Repository, Service, Utility, WebAPI, and Tests projects.
- **Entity Framework Core**: Code-first approach with migrations for robust data access.
- **Dependency Injection**: Promotes loose coupling and testability.
- **Command/Query Responsibility Segregation (CQRS)**: Clear separation between read and write operations.
- **Exception Handling**: Centralized and consistent error management.
- **Unit Testing**: Ensures code quality and reliability.
- **.NET 8.0**: Leverages the latest features and performance improvements.

## Project Structure

- `Products.Common`: Shared entities and exceptions.
- `Products.Repository`: Data access layer using EF Core.
- `Products.Service`: Business logic and command/query handlers.
- `Products.Utility`: Helper utilities.
- `Products.WebAPI`: RESTful API endpoints.
- `Products.Tests`: Unit and integration tests.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB)
- (Optional) [Visual Studio 2022+](https://visualstudio.microsoft.com/)

## Setup Instructions

1. **Clone the Repository**
   ```sh
   git clone <repo-url>
   cd ProductManagement
   ```

2. **Restore Dependencies**
   ```sh
   dotnet restore
   ```

3. **Configure Database Connection**
   - Update the connection string in `Products.WebAPI/appsettings.json` or `Products.Repository/appsettings.json` as needed.

4. **Apply Database Migrations**
   - Navigate to the repository project:
     ```sh
     cd Products.Repository
     ```
   - Run the following commands to create and update the database:
     ```sh
     dotnet ef database update
     ```
   - This will apply the latest migrations and seed initial data.

5. **Build the Solution**
   ```sh
   dotnet build
   ```

6. **Run the Web API**
   ```sh
   cd ../Products.WebAPI
   dotnet run
   ```
   - The API will be available at `https://localhost:5001` or as configured.

7. **Run Tests**
   ```sh
   cd ../Products.Tests
   dotnet test
   ```

