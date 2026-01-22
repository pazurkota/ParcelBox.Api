using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ParcelBox.Api.Tests;

public class BoxesApiTest : IClassFixture<WebApplicationFactory<Program>>
{
    private const string BaseUrl = "api/boxes";
    private readonly HttpClient _client;

    public BoxesApiTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        TestSeeder.SeedDefaultLockers(factory.Services);
    }

    [Fact]
    public async Task AddLockerBoxes_ReturnsOkResult()
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/add/1", TestData.CreateLockerBoxDtos());

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddLockerBoxes_ReturnsBadRequest()
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/add/1", TestData.CreateInvalidLockerBoxDtos());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task AddLockerBoxes_ReturnsNotFound()
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/add/0", TestData.CreateLockerBoxDtos());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

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
}