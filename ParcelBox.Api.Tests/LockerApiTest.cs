using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public class LockerApiTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private const string BaseUrl = "api/lockers";
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAllLockers_ReturnsOkResult()
    {
        var response = await _client.GetAsync(BaseUrl);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllLockers_WithQuery_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{BaseUrl}?page=1");

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetAllLockers_WithQuery_ReturnsBadRequest()
    {
        var response = await _client.GetAsync($"{BaseUrl}?page=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
    /*
    [Fact]
    public async Task CreateLocker_ReturnsCreatedResult()
    {
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", TestData.CreateLockerDto());

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateLocker_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/create", TestData.InvalidCreateLockerDto());
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EditLocker_ReturnsOkResult()
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/1/edit", TestData.EditLockerDto());

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task EditLocker_ReturnsBadRequest()
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/1/edit", TestData.InvalidEditLockerDto());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task EditLocker_ReturnsNotFound()
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/0/edit", TestData.EditLockerDto());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
*/
}