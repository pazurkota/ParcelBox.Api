using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests.BoxesTests;

public class BoxTestHandler(CustomWebApplicationFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    private static int _lockerCounter = 0;

    public async Task<int> CreateLockerAsync()
    {
        var counter = Interlocked.Increment(ref _lockerCounter);
        var code = $"BXT-{counter:D3}";

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
        return createdLocker!.Id;
    }

    public async Task<(int LockerId, List<LockerBox> Boxes)> CreateLockerWithBoxesAsync()
    {
        var lockerId = await CreateLockerAsync();

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
        var createdBoxes = await boxesResponse.Content.ReadFromJsonAsync<List<LockerBox>>();
        return (lockerId, createdBoxes!);
    }
}
