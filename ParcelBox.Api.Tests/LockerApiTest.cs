using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public class LockerApiTest : IClassFixture<WebApplicationFactory<Program>>
{
    private const string BaseUrl = "api/lockers";
    
    private readonly HttpClient _client;
    
    private readonly Locker _testLocker = new()
    {
        Id = 1,
        Code = "WAS-002",
        Address = "1600 Pennsylvania Avenue NW",
        City = "Washington DC",
        PostalCode = "20500 USA"
    };

    private readonly CreateLockerBoxDto[] _lockerBoxes = new[]
    {
        new CreateLockerBoxDto { LockerSize = "Small" },
        new CreateLockerBoxDto {LockerSize = "Medium"}
    };

    public LockerApiTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        var repository = factory.Services.GetRequiredService<IRepository<Locker>>();
        repository.Create(new Locker()
        {
            Id = 1,
            Address = "1400 Defense Pentagon",
            City = "Washington DC",
            PostalCode = "20301 USA",
            Code = "WAS-001",
            LockerBoxes = []
        });
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
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", _testLocker);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateLocker_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", new Locker
        {
            Code = " ",
            Address = " ",
            City = " ",
            PostalCode = " "
        });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateLockerBoxes_ReturnsCreatedResult()
    {
        var response = await _client.PatchAsJsonAsync($"{BaseUrl}/1/boxes", _lockerBoxes);

        response.EnsureSuccessStatusCode();
    }
}