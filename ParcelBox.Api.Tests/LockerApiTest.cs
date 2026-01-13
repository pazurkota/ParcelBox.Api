using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public class LockerApiTest : IClassFixture<WebApplicationFactory<Program>>
{
    private const string BaseUrl = "api/lockers";
    
    private readonly HttpClient _client;
    private readonly Locker _testLocker = new()
    {
        Id = 1,
        Address = "1400 Defense Pentagon",
        City = "Washington DC",
        PostalCode = "20301 USA",
        Code = "WAS-001",
        LockerBoxes =
        [
            new LockerBox
            {
                Id = 1,
                LockerSize = nameof(LockerSize.Small),
                IsOccupied = false,
                LockerId = 1
            },

            new LockerBox
            {
                Id = 2,
                LockerSize = nameof(LockerSize.Medium),
                IsOccupied = true,
                LockerId = 1
            },

            new LockerBox
            {
                Id = 3,
                LockerSize = nameof(LockerSize.Big),
                IsOccupied = false,
                LockerId = 1
            }
        ]
    };

    public LockerApiTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        var repository = factory.Services.GetRequiredService<IRepository<Locker>>();
        repository.Create(_testLocker);
    }

    [Fact]
    public async Task GetAllLockers_ReturnsOkResult()
    {
        var response = await _client.GetAsync(BaseUrl);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetLockerById_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{BaseUrl}/{_testLocker.Id}");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetLockerById_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{BaseUrl}/{_testLocker.Id + 1}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}