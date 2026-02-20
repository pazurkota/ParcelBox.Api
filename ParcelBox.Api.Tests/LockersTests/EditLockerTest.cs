using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests.LockersTests;

public class EditLockerTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task EditLocker_ReturnsOkResult()
    {
        // Arrange
        var newLocker = new CreateLockerDto
        {
            Code = "EDT-001",
            Address = "Test St",
            City = "Test City",
            PostalCode = "00-000"
        };

        var createResponse = await _client.PostAsJsonAsync($"{ApiLinks.LockersUrl}/create", newLocker);
        createResponse.EnsureSuccessStatusCode();

        var createdLocker = await createResponse.Content.ReadFromJsonAsync<Locker>();
        var idToEdit = createdLocker!.Id;

        EditLockerDto editLocker = new()
        {
            Address = "800 Southern Ave SE"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{ApiLinks.LockersUrl}/{idToEdit}/edit", editLocker);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task EditLocker_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PutAsJsonAsync($"{ApiLinks.LockersUrl}/1/edit", new CreateLockerDto());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EditLocker_ReturnsNotFound()
    {
        // Arrange
        EditLockerDto editLocker = new()
        {
            Address = "800 Southern Ave SE"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{ApiLinks.LockersUrl}/0/edit", editLocker);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
