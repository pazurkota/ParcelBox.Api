# ParcelBox.Api

*A simple RESTful API for managing parcel boxes and lockers built with ASP.NET Core.*

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/pazurkota/ParcelBox.Api/dotnet.yml)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/pazurkota/ParcelBox.Api/generate-docs.yml?label=docs%20build)
![GitHub License](https://img.shields.io/github/license/pazurkota/ParcelBox.Api)
![GitHub Version](https://img.shields.io/github/v/tag/pazurkota/ParcelBox.api?label=version)

## Features

- Create and manage parcel lockers
- Add locker boxes with different sizes (Small, Medium, Big)
- Retrieve locker information by ID or get all lockers
- Update locker details
- Input validation with FluentValidation
- Interactive API documentation with Swagger
- Full integration test coverage

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

### Clone the Repository

```bash
git clone https://github.com/pazurkota/ParcelBox.Api.git
cd ParcelBox.Api
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
