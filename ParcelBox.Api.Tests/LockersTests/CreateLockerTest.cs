using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Locker;

namespace ParcelBox.Api.Tests.LockersTests;

public class CreateLockerTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateLocker_ReturnsCreatedResult()
    {
        // Arrange
        CreateLockerDto newLocker = new()
        {
            Code = "WAS-002",
            Address = "1600 Pennsylvania Avenue NW",
            City = "Washington DC",
            PostalCode = "20500 USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiLinks.LockersUrl}/create", newLocker);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateLocker_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync($"{ApiLinks.LockersUrl}/create", new CreateLockerDto());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
