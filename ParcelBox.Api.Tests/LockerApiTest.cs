using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public class LockerApiTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private const string BaseUrl = "api/lockers";
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAllLockers_ReturnsOkResult()
    {
        var response = await _client.GetAsync(BaseUrl);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllLockers_WithQuery_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{BaseUrl}?page=1");

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetAllLockers_WithQuery_ReturnsBadRequest()
    {
        var response = await _client.GetAsync($"{BaseUrl}?page=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetLockerById_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{BaseUrl}/1");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetLockerById_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{BaseUrl}/0");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

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
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", newLocker);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateLocker_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", new CreateLockerDto());
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    
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
    
        var createResponse = await _client.PostAsJsonAsync($"{BaseUrl}/create", newLocker);
        createResponse.EnsureSuccessStatusCode();
        
        var createdLocker = await createResponse.Content.ReadFromJsonAsync<Locker>(); 
        var idToEdit = createdLocker!.Id; 
        
        EditLockerDto editLocker = new()
        {
            Address = "800 Southern Ave SE"
        };
    
        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{idToEdit}/edit", editLocker);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task EditLocker_ReturnsBadRequest()
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/1/edit", new CreateLockerDto());

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
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/0/edit", editLocker);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteLocker_RemovesLockerAndBoxes()
    {
        var newLocker = new CreateLockerDto
        {
            Code = "TST-003",
            Address = "Test Address",
            City = "Test City",
            PostalCode = "00-000"
        };

        var createResponse = await _client.PostAsJsonAsync($"{BaseUrl}/create", newLocker);
        createResponse.EnsureSuccessStatusCode();
        var createdLocker = await createResponse.Content.ReadFromJsonAsync<Locker>();

        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{createdLocker!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"{BaseUrl}/{createdLocker.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteLocker_ReturnsNotFoundForNonExistentLocker()
    {
        var response = await _client.DeleteAsync($"{BaseUrl}/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}