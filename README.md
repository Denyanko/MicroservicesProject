# Mini Microservices Project

## Overview

Welcome to the Mini Microservices Project! This project demonstrates the implementation of a simple microservices architecture using ASP.NET, .NET 6, EF Core, Swagger, AutoMapper, and MediatR, with a focus on the CQRS (Command Query Responsibility Segregation) pattern. The project consists of two microservices: Student Service and Book Service, each with its own set of functionalities and databases.

## Microservices

### 1. Student Service

The Student Service manages information related to students and their parent/guardian. It consists of two tables:

- **Student**: This table holds information about individual students.
- **Parent**: This table stores information about the parent or guardian of a student.

#### One-to-One Relationship

The Student and Parent tables have a one-to-one relationship, where each student is associated with one parent/guardian.

### 2. Book Service

The Book Service is responsible for managing information related to books, authors, and genres. It comprises three tables:

- **Book**: This table stores details about individual books.
- **Author**: This table contains information about the authors of the books.
- **Genre**: This table holds data about book genres.

#### One-to-Many Relationship

The Book and Author tables have a one-to-many relationship, where each book can have one author, but an author can be associated with multiple books.

#### Many-to-Many Relationship

The Book and Genre tables have a many-to-many relationship, which is achieved through a junction table called **BookGenre**. This allows each book to have multiple genres, and each genre can be associated with multiple books.

## Technologies Used

This project leverages several technologies and libraries to facilitate the development and management of microservices:

- **ASP.NET**: The framework used to build the microservices.
- **.NET 6**: The version of the .NET framework.
- **EF Core (Entity Framework Core)**: Used for database access and management.
- **Swagger**: Provides API documentation and testing capabilities.
- **AutoMapper**: Simplifies object-to-object mapping.
- **MediatR**: Implements the Mediator pattern for CQRS support.

## Getting Started

To get started with this project, follow these steps:

1. Clone the repository to your local machine.

2. Set up the database configurations for both the Student Service and Book Service, ensuring they are correctly configured in the respective microservices.

3. Run the microservices locally or deploy them to your desired hosting environment.

4. Use Swagger documentation to explore and interact with the APIs of both microservices.

5. Explore the CQRS pattern implementation in the codebase to understand how commands and queries are handled.

## API Documentation

Each microservice is equipped with Swagger documentation that provides detailed information about the available endpoints, request/response schemas, and allows for easy testing of API functionalities.

### Student Service Swagger URL

- http://localhost:portNumber/swagger/index.html (Replace "portNumber" with the actual port where the Student Service is running.)

### Book Service Swagger URL

- http://localhost:portNumber/swagger/index.html (Replace "portNumber" with the actual port where the Book Service is running.)

## Contributing

If you would like to contribute to this project, please follow the standard GitHub workflow:

1. Fork the repository.

2. Create a new branch for your feature or bug fix.

3. Make your changes and commit them.

4. Push your changes to your forked repository.

5. Create a pull request to the main repository, describing your changes and why they should be merged.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

If you have any questions or need further assistance, please feel free to contact the project maintainers:

- [Ade Widyatama Dian Boernama](mailto:adewidyatamadb@gmail.com)

Thank you for using and contributing to the Mini Microservices Project!
