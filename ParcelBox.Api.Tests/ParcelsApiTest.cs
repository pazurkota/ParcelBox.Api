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