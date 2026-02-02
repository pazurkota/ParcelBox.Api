using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public class BoxesApiTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private const string BaseUrl = "api/boxes";
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task AddLockerBoxes_ReturnsOkResult()
    {
        // Arrange
        var newLocker = new CreateLockerDto
        {
            Code = "TST-001",
            Address = "Test St",
            City = "Test City",
            PostalCode = "00-000"
        };

        var createLockers = new CreateLockerBoxesDtos
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() {LockerSize = "Small"},
                new() {LockerSize = "Medium"},
                new() {LockerSize = "Big"}
            }
        };
    
        var createResponse = await _client.PostAsJsonAsync("api/lockers/create", newLocker);
        createResponse.EnsureSuccessStatusCode();
        
        var createdLocker = await createResponse.Content.ReadFromJsonAsync<Locker>(); 
        var idToEdit = createdLocker!.Id; 
        
        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/add/{idToEdit}", createLockers);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddLockerBoxes_ReturnsBadRequest()
    {
        // Arrange
        var newLocker = new CreateLockerDto
        {
            Code = "TST-001",
            Address = "Test St",
            City = "Test City",
            PostalCode = "00-000"
        };
    
        var createResponse = await _client.PostAsJsonAsync("api/lockers/create", newLocker);
        createResponse.EnsureSuccessStatusCode();
        
        var createdLocker = await createResponse.Content.ReadFromJsonAsync<Locker>(); 
        var idToEdit = createdLocker!.Id; 
        
        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/add/{idToEdit}", new CreateLockerBoxesDtos());
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task AddLockerBoxes_ReturnsNotFound()
    {
        // Arrange
        var createLockers = new CreateLockerBoxesDtos
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() {LockerSize = "Small"},
                new() {LockerSize = "Medium"},
                new() {LockerSize = "Big"}
            }
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/add/0", createLockers);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
/*
    [Fact]
    public async Task EditLockerBoxStatus_ReturnsOkResult()
    {
        var response = await _client.PatchAsJsonAsync($"{BaseUrl}/occupied/1/1/true", "");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsNotFound()
    {
        var response = await _client.PatchAsJsonAsync($"{BaseUrl}/occupied/2/1/true", "");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
*/
}