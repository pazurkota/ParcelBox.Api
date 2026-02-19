using System.Net;

namespace ParcelBox.Api.Tests.LockersTests;

public class GetLockerByIdTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetLockerById_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{ApiLinks.LockersUrl}/1");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetLockerById_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{ApiLinks.LockersUrl}/0");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
