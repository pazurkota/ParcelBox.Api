using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Parcel;

namespace ParcelBox.Api.Tests.ParcelsTests;

public class CreateParcelTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory> 
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly ParcelTestHandler _handler = new(factory);
    
    [Fact]
    public async Task CreateParcel_ReturnsCreatedResult()
    {
        // Arrange - Create two lockers with boxes
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", newParcel);

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
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "InvalidSize",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", newParcel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateParcel_WithBigSize_ReturnsCreatedResult()
    {
        // Arrange
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Big",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", newParcel);

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
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", newParcel);

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
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", newParcel);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdParcel = await response.Content.ReadFromJsonAsync<GetParcelDto>();
        
        Assert.NotNull(createdParcel);
        Assert.NotEmpty(createdParcel.PickupCode);
        Assert.Equal(8, createdParcel.PickupCode.Length);
    }
}