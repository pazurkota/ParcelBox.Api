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
    public async Task GetAllParcels_ReturnsOkResult()
    {
        var response = await _client.GetAsync(BaseUrl);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllParcels_WithQuery_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{BaseUrl}?page=1&recordsPerPage=10");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllParcels_WithSizeFilter_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{BaseUrl}?size=Small");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllParcels_WithInvalidSize_ReturnsBadRequest()
    {
        var response = await _client.GetAsync($"{BaseUrl}?size=InvalidSize");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetParcelById_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{BaseUrl}/999999");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
    public async Task GetParcelById_AfterCreate_ReturnsOkResult()
    {
        // Arrange
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Medium",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        var createResponse = await _client.PostAsJsonAsync($"{BaseUrl}/create", newParcel);
        createResponse.EnsureSuccessStatusCode();
        var createdParcel = await createResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        // Act
        var response = await _client.GetAsync($"{BaseUrl}/{createdParcel!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var parcel = await response.Content.ReadFromJsonAsync<GetParcelDto>();
        Assert.NotNull(parcel);
        Assert.Equal(createdParcel.Id, parcel.Id);
        Assert.Equal("Medium", parcel.ParcelSize);
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
    public async Task GetAllParcels_WithPagination_ReturnsCorrectPage()
    {
        // Arrange - Create two locker boxes
        var initialLockerId = await CreateLockerWithBoxesAsync();
        var targetLockerId = await CreateLockerWithBoxesAsync();

        for (int i = 0; i < 3; i++)
        {
            var parcel = new CreateParcelDto
            {
                ParcelSize = "Small",
                InitialLockerId = initialLockerId,
                TargetLockerId = targetLockerId
            };
            var createResponse = await _client.PostAsJsonAsync($"{BaseUrl}/create", parcel);
            createResponse.EnsureSuccessStatusCode();
        }

        // Act
        var response = await _client.GetAsync($"{BaseUrl}?page=1&recordsPerPage=2");

        // Assert
        response.EnsureSuccessStatusCode();
        var parcels = await response.Content.ReadFromJsonAsync<List<GetParcelDto>>();
        Assert.NotNull(parcels);
        Assert.True(parcels.Count <= 2);
    }
}