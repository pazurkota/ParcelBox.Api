using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests.ParcelsTests;

public class ParcelTestHandler(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static int _lockerCounter = 0;
    
    public async Task<int> CreateLockerWithBoxesAsync()
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

        var lockerResponse = await _client.PostAsJsonAsync($"{ApiLinks.LockersUrl}/create", locker);
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

        var boxesResponse = await _client.PutAsJsonAsync($"{ApiLinks.BoxesUrl}/add/{lockerId}", boxes);
        boxesResponse.EnsureSuccessStatusCode();

        return lockerId;
    }
}