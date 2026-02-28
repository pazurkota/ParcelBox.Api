using System.Net;

namespace ParcelBox.Api.Tests.LockersTests;

public class GetAllLockersTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAllLockers_ReturnsOkResult()
    {
        var response = await _client.GetAsync(ApiLinks.LockersUrl);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllLockers_WithQuery_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{ApiLinks.LockersUrl}?page=1");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllLockers_WithQuery_ReturnsBadRequest()
    {
        var response = await _client.GetAsync($"{ApiLinks.LockersUrl}?page=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
