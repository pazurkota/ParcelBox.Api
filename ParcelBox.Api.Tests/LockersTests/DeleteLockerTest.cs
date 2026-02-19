using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests.LockersTests;

public class DeleteLockerTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task DeleteLocker_RemovesLockerAndBoxes()
    {
        // Arrange
        var newLocker = new CreateLockerDto
        {
            Code = "TST-003",
            Address = "Test Address",
            City = "Test City",
            PostalCode = "00-000"
        };

        var createResponse = await _client.PostAsJsonAsync($"{ApiLinks.LockersUrl}/create", newLocker);
        createResponse.EnsureSuccessStatusCode();
        var createdLocker = await createResponse.Content.ReadFromJsonAsync<Locker>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"{ApiLinks.LockersUrl}/{createdLocker!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"{ApiLinks.LockersUrl}/{createdLocker.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteLocker_ReturnsNotFoundForNonExistentLocker()
    {
        // Act
        var response = await _client.DeleteAsync($"{ApiLinks.LockersUrl}/999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
