using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Dtos.Parcel;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public class ParcelsApiTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private const string BaseUrl = "api/parcels";
    private const string LockersUrl = "api/lockers";
    private const string BoxesUrl = "api/boxes";
    private readonly HttpClient _client = factory.CreateClient();

    private static int _lockerCounter = 0;
    
    private async Task<int> CreateLockerWithBoxesAsync()
    {
        var counter = Interlocked.Increment(ref _lockerCounter);
        var code = $"TST-{counter:D3}";
        
        // Create new locker
        var locker = new CreateLockerDto
        {
            Code = code,
            Address = "Test Address",
            City = "Test City",
            PostalCode = "00-000"
        };

        var lockerResponse = await _client.PostAsJsonAsync($"{LockersUrl}/create", locker);
        lockerResponse.EnsureSuccessStatusCode();
        var createdLocker = await lockerResponse.Content.ReadFromJsonAsync<Locker>();
        var lockerId = createdLocker!.Id;

        // Add boxes to locker
        var boxes = new CreateLockerBoxesDtos
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() { LockerSize = "Small" },
                new() { LockerSize = "Medium" },
                new() { LockerSize = "Big" }
            }
        };

        var boxesResponse = await _client.PutAsJsonAsync($"{BoxesUrl}/add/{lockerId}", boxes);
        boxesResponse.EnsureSuccessStatusCode();

        return lockerId;
    }

    [Fact]
    public async Task CreateParcel_ReturnsCreatedResult()
    {
        // Arrange - Create two lockers with boxes
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdParcel = await response.Content.ReadFromJsonAsync<GetParcelDto>();
        Assert.NotNull(createdParcel);
        Assert.Equal("Small", createdParcel.ParcelSize);
        Assert.Equal(initialLockerId, createdParcel.InitialLockerId);
        Assert.Equal(targetLockerId, createdParcel.TargetLockerId);
        Assert.NotEmpty(createdParcel.PickupCode);
    }

    [Fact]
    public async Task CreateParcel_WithInvalidSize_ReturnsBadRequest()
    {
        // Arrange
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "InvalidSize",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateParcel_WithBigSize_ReturnsCreatedResult()
    {
        // Arrange
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Big",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdParcel = await response.Content.ReadFromJsonAsync<GetParcelDto>();
        Assert.NotNull(createdParcel);
        Assert.Equal("Big", createdParcel.ParcelSize);
    }

    [Fact]
    public async Task CreateParcel_AssignsLockerBoxIds()
    {
        // Arrange
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdParcel = await response.Content.ReadFromJsonAsync<GetParcelDto>();
        
        Assert.NotNull(createdParcel);
        Assert.True(createdParcel.InitialLockerBoxId > 0);
        Assert.True(createdParcel.TargetLockerBoxId > 0);
    }

    [Fact]
    public async Task CreateParcel_GeneratesPickupCode()
    {
        // Arrange
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdParcel = await response.Content.ReadFromJsonAsync<GetParcelDto>();
        
        Assert.NotNull(createdParcel);
        Assert.NotEmpty(createdParcel.PickupCode);
        Assert.Equal(8, createdParcel.PickupCode.Length);
    }

    [Fact]
    public async Task EditParcel_UpdatesTargetLockerAndStatus()
    {
        // Arrange
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        var createResponse = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);
        createResponse.EnsureSuccessStatusCode();
        var createdParcel = await createResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        var editDto = new EditParcelDto
        {
            TargetLockerId = targetLockerId,
            ParcelStatus = "OnTheWay"
        };

        // Act
        var editResponse = await _client.PutAsJsonAsync($"{BaseUrl}/{createdParcel!.Id}/edit", editDto);

        // Assert
        editResponse.EnsureSuccessStatusCode();
        var updatedParcelResponse = await _client.GetAsync($"{BaseUrl}/{createdParcel.Id}");
        updatedParcelResponse.EnsureSuccessStatusCode();
        var updatedParcel = await updatedParcelResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        Assert.NotNull(updatedParcel);
        Assert.Equal("OnTheWay", updatedParcel.ParcelStatus);
        Assert.Equal(targetLockerId, updatedParcel.TargetLockerId);
    }

    [Fact]
    public async Task EditParcel_ReturnsNotFoundForInvalidId()
    {
        // Arrange
        var editDto = new EditParcelDto
        {
            TargetLockerId = 9999,
            ParcelStatus = "Delivered"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/999999/edit", editDto);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task EditParcel_ReturnsBadRequestForInvalidStatus()
    {
        // Arrange
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        var createResponse = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);
        createResponse.EnsureSuccessStatusCode();
        var createdParcel = await createResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        var editDto = new EditParcelDto
        {
            ParcelStatus = "InvalidStatus"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{createdParcel!.Id}/edit", editDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteParcel_RemovesParcelAndReturnsNoContent()
    {
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        var createResponse = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);
        createResponse.EnsureSuccessStatusCode();
        var createdParcel = await createResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{createdParcel!.Id}/delete");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"{BaseUrl}/{createdParcel.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteParcel_ReturnsNotFoundForNonExistentParcel()
    {
        var response = await _client.DeleteAsync($"{BaseUrl}/999999/delete");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}