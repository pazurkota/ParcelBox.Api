using System.Net;

namespace ParcelBox.Api.Tests.ParcelsTests;

public class GetAllParcelsApiTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory> 
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task GetAllParcels_ReturnsOkResult()
    {
        var response = await _client.GetAsync(ApiLinks.ParcelsUrl);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllParcels_WithQuery_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{ApiLinks.ParcelsUrl}?page=1&recordsPerPage=10");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllParcels_WithSizeFilter_ReturnsOkResult()
    {
        var response = await _client.GetAsync($"{ApiLinks.ParcelsUrl}?size=Small");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllParcels_WithInvalidSize_ReturnsBadRequest()
    {
        var response = await _client.GetAsync($"{ApiLinks.ParcelsUrl}?size=InvalidSize");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}