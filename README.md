# StockApp API: A Layered API Implementation with .NET 9 and Dapper

## 1. Project Overview

This project is a practical implementation of a RESTful API for a stock and product management system, built on .NET 9. The primary objective is to develop a robust, high-performance backend service by applying industry-standard architectural patterns and modern development techniques. The project serves as a comprehensive portfolio piece demonstrating proficiency in building scalable and maintainable APIs.

## 2. Architectural Design

The application is built upon a Layered Architecture (N-Tier) to enforce a strong Separation of Concerns (SoC). This design isolates responsibilities, making the application easier to maintain, test, and scale.

### API Layer (Presentation)
The entry point of the application, built with ASP.NET Core. Its sole responsibility is to handle HTTP requests and responses. It receives requests, forwards them to the Service Layer, and returns the appropriate HTTP status codes (e.g., 200 OK, 201 Created, 404 Not Found) and data to the client.

### Infrastructure Layer
Contains the implementation details for external concerns.
- **Service Layer:** Acts as the application's core logic unit. It orchestrates business processes, validates data, and handles the mapping between DTOs and database entities. Error handling (try-catch) and detailed logging (ILogger) are centralized here.
- **Repository Layer:** The only layer that communicates directly with the database. It uses Dapper to execute raw SQL queries and abstracts all data access logic away from the rest of the application.

### Core Layer
The heart of the application, with no dependencies on any other layer.
- **Data Models (Entities):** Simple C# classes (Product, Category) that represent the database tables. They contain no logic.
- **DTOs (Data Transfer Objects):** Defines the API's public contract (ProductDto, CreateProductDto, etc.). These objects are used to shape the data sent to and from the API, decoupling the client from the internal database structure.
- **Interfaces:** Contracts (IProductRepository, IProductService) that define the capabilities of the Repository and Service layers, enabling Dependency Inversion and facilitating unit testing.

## 3. Key Technical Concepts Implemented

This project provided an opportunity to implement and master the following modern .NET and database techniques:

### High-Performance Data Access with Dapper
The Dapper Micro ORM was chosen over Entity Framework Core to achieve maximum performance and retain full control over SQL execution.
- **Multi-Mapping:** Implemented to efficiently query and map relational data from SQL JOINs (e.g., Product + Category) into nested C# object graphs (product.Category) within a single database request, solving the common N+1 query problem.
- **QueryMultiple Alternative:** Utilized multiple, sequential QueryAsync calls on a single connection as a readable and maintainable strategy to build complex data objects (as seen in the summary endpoint), serving as a practical alternative to QueryMultiple in complex mapping scenarios.
- **Transaction Management:** Implemented database Transactions (BeginTransactionAsync, CommitAsync, RollbackAsync) for "all-or-nothing" operations like stock updates. This ensures data integrity and atomicity by guaranteeing that multi-step database modifications are completed successfully or not at all.

### Layered Architecture Best Practices
- **Centralized Error Handling:** try-catch blocks are strategically placed in the Service Layer to catch and log specific data access exceptions thrown by the Repository. Controllers remain clean of this logic.
- **Structured Logging (ILogger):** ILogger is used throughout the application to provide context-rich, structured logs. LogInformation is used in Controllers for request tracing, while LogError is used in the Service layer to record detailed exception information.

### ASP.NET Core API Implementation Details
- **DTO Pattern ("API as a Contract"):** The API strictly uses DTOs for all incoming and outgoing data, ensuring the internal database models are never exposed to the client. All DTO-to-Entity mapping logic is handled within the Service Layer.
- **Type-Safe Configuration (Options Pattern):** Application settings are bound to the `DatabaseSettings` POCO class using the Options Pattern. This class holds the connection string, avoiding the use of "magic strings" with IConfiguration and making configuration access safer and easier to test.
- **RESTful CreatedAtAction:** Implemented CreatedAtAction to return a 201 Created status with a Location header upon successful resource creation, adhering to RESTful API standards. The nameof operator is used to provide a type-safe reference to the "get" action, preventing runtime errors from magic strings.

## 4. Technology Stack

- **Framework:** .NET 9
- **API:** ASP.NET Core REST API
- **Database:** PostgreSQL (Running on Docker)
- **Data Access:** Dapper (Micro ORM) & Npgsql

## 5. Setup and Execution

### Prerequisites
- .NET SDK (9.0 or higher)
- Docker

### Database Setup (Docker)
1.  Run the PostgreSQL container using the `docker run` command in your terminal. This will start the database in the background.
    ```sh
    docker run -d --name stockapp_db -e POSTGRES_USER=myuser -e POSTGRES_PASSWORD=mypassword -e POSTGRES_DB=StockAppDB -p 5432:5432 postgres
    ```
2.  Edit the `ConnectionStrings` section in the `StockApp.API/appsettings.json` file to match the credentials provided in the command above.
3.  Execute the `database.sql` script against the `StockAppDB` database using a database management tool (e.g., DBeaver, DataGrip) to create the necessary tables.

### Application Launch
1.  Navigate to the `StockApp.API` folder in your terminal.
2.  Run the application using the following command:
    ```sh
    dotnet run