# ParcelBox.Api

*A simple RESTful API for managing parcel boxes and lockers built with ASP.NET Core.*

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/pazurkota/ParcelBox.Api/dotnet.yml)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/pazurkota/ParcelBox.Api/generate-docs.yml?label=docs%20build)
![GitHub License](https://img.shields.io/github/license/pazurkota/ParcelBox.Api)
![GitHub Version](https://img.shields.io/github/v/tag/pazurkota/ParcelBox.api?label=version)

## Features

- **Locker Management**
  - Create, edit and delete parcel lockers
  - Retrieve locker information by ID or get all lockers with pagination
  - Filter lockers by city or postal code
- **Locker Box Management**
  - Add locker boxes with different sizes (Small, Medium, Big)
  - Update box occupancy status
- **Developer Experience**
  - Input validation with FluentValidation
  - Interactive API documentation with Swagger
  - Full integration test coverage
  - Docker support for easy deployment

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- [PostreSQL 18](https://www.postgresql.org/download/) or later
- [Docker](https://www.docker.com/get-started) (optional, for containerized deployment)

### Clone the Repository

```bash
git clone https://github.com/pazurkota/ParcelBox.Api.git
cd ParcelBox.Api
```

### Running with Docker (Recommended)

1. **Configure environment variables:**

   Create a `.env` file in the root directory with the following content:

   ```dotenv
   DB_PASSWORD=your_secure_password
   DB_USER=postgres
   DB_NAME=parcelbox_db
   ```

2. **Start the containers:**

   ```bash
   docker-compose up
   ```

   This will start:
   - PostgreSQL database on port `5432`
   - ASP.NET Core API on port `8080`

3. **Access the API:**

   Open your browser and navigate to `http://localhost:8080/swagger` to view the interactive API documentation.

4. **Stop the containers:**

   ```bash
   docker-compose down
   ```

### Running Locally

1. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

2. **Build the project:**

   ```bash
   dotnet build
   ```

3. **Run the API:**

   ```bash
   cd ParcelBox.Api
   dotnet run -lp https
   ```

    The API will be available at `https://localhost:7142` (or `http://localhost:5160`).

4. **Access the API documentation:**

   Open your browser and navigate to `https://localhost:7142/swagger` to view the interactive API documentation.

### Running Tests

```bash
dotnet test
```

## Contributing

Contributions are welcome! Here's how you can help:

1. **Fork the repository**
2. **Create a feature branch:** `git checkout -b feature/your-feature-name`
3. **Commit your changes:** `git commit -m 'Add some feature'`
4. **Push to the branch:** `git push origin feature/your-feature-name`
5. **Open a Pull Request**

Please ensure your code:

- Follows existing code style and conventions
- Includes appropriate tests
- Updates documentation as needed

## Support

If you encounter any issues or have questions:

- **Issues:** [GitHub Issues](https://github.com/pazurkota/ParcelBox.Api/issues)
- **Documentation:** [API Docs](https://pazurkota.github.io/ParcelBox.Api/)
- **Discussions:** Feel free to open a discussion for general questions

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
