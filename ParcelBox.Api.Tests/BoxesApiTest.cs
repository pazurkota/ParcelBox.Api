using System.Net;
using System.Net.Http.Json;
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

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsOkResult_WhenSettingOccupiedToTrue()
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
    
        // create and check test locker
        var createResponse = await _client.PostAsJsonAsync("api/lockers/create", newLocker);
        createResponse.EnsureSuccessStatusCode();
        
        // get locker id
        var createdLocker = await createResponse.Content.ReadFromJsonAsync<Locker>(); 
        var idToEdit = createdLocker!.Id; 
        
        // create new locker boxes
        var createLockerBoxes = await _client
            .PutAsJsonAsync($"{BaseUrl}/add/{idToEdit}", createLockers);
        createLockerBoxes.EnsureSuccessStatusCode();

        var createdLockerBoxes = await createLockerBoxes.Content.ReadFromJsonAsync<List<LockerBox>>();
        var boxIdToEdit = createdLockerBoxes![0].Id;
        
        // Act
        var response = await _client
            .PatchAsJsonAsync($"{BaseUrl}/occupied?boxId={boxIdToEdit}&isOccupied=true", "");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsOkResult_WhenSettingOccupiedToFalse()
    {
        // Arrange
        var newLocker = new CreateLockerDto
        {
            Code = "TST-002",
            Address = "Test St",
            City = "Test City",
            PostalCode = "00-000"
        };

        var createLockers = new CreateLockerBoxesDtos
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() {LockerSize = "Small"}
            }
        };
    
        var createResponse = await _client.PostAsJsonAsync("api/lockers/create", newLocker);
        createResponse.EnsureSuccessStatusCode();
        
        var createdLocker = await createResponse.Content.ReadFromJsonAsync<Locker>(); 
        var idToEdit = createdLocker!.Id; 
        
        var createLockerBoxes = await _client
            .PutAsJsonAsync($"{BaseUrl}/add/{idToEdit}", createLockers);
        createLockerBoxes.EnsureSuccessStatusCode();

        var createdLockerBoxes = await createLockerBoxes.Content.ReadFromJsonAsync<List<LockerBox>>();
        var boxIdToEdit = createdLockerBoxes![0].Id;
        
        // First set to occupied
        await _client.PatchAsJsonAsync($"{BaseUrl}/occupied?boxId={boxIdToEdit}&isOccupied=true", "");
        
        // Act - set back to not occupied
        var response = await _client
            .PatchAsJsonAsync($"{BaseUrl}/occupied?boxId={boxIdToEdit}&isOccupied=false", "");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsNotFound_WhenBoxIdDoesNotExist()
    {
        // Arrange
        var nonExistentBoxId = 99999;
        
        // Act
        var response = await _client
            .PatchAsJsonAsync($"{BaseUrl}/occupied?boxId={nonExistentBoxId}&isOccupied=true", "");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsBadRequest_WhenBoxIdIsMissing()
    {
        // Act
        var response = await _client
            .PatchAsJsonAsync($"{BaseUrl}/occupied?isOccupied=true", "");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsBadRequest_WhenBoxIdIsZero()
    {
        // Act
        var response = await _client
            .PatchAsJsonAsync($"{BaseUrl}/occupied?boxId=0&isOccupied=true", "");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsBadRequest_WhenBoxIdIsNegative()
    {
        // Act
        var response = await _client
            .PatchAsJsonAsync($"{BaseUrl}/occupied?boxId=-1&isOccupied=true", "");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteLockerBox_RemovesBoxAndReturnsNoContent()
    {
        var newLocker = new CreateLockerDto
        {
            Code = "TST-002",
            Address = "Test Address",
            City = "Test City",
            PostalCode = "00-000"
        };

        var createLockerResponse = await _client.PostAsJsonAsync("api/lockers/create", newLocker);
        createLockerResponse.EnsureSuccessStatusCode();
        var createdLocker = await createLockerResponse.Content.ReadFromJsonAsync<Locker>();

        var lockerBox = new CreateLockerBoxesDtos
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() { LockerSize = "Small" }
            }
        };

        var addBoxResponse = await _client.PutAsJsonAsync($"{BaseUrl}/add/{createdLocker!.Id}", lockerBox);
        addBoxResponse.EnsureSuccessStatusCode();
        var createdBoxes = await addBoxResponse.Content.ReadFromJsonAsync<List<LockerBox>>();

        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{createdBoxes![0].Id}/delete");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"{BaseUrl}/{createdBoxes[0].Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteLockerBox_ReturnsNotFound_WhenBoxDoesNotExist()
    {
        var response = await _client.DeleteAsync($"{BaseUrl}/999999/delete");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}