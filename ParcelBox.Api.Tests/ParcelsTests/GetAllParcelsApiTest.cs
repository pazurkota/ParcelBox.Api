using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Parcel;

namespace ParcelBox.Api.Tests.ParcelsTests;

public class GetAllParcelsApiTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory> 
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly ParcelTestHandler _handler = new(factory);
    
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
    
    [Fact]
    public async Task GetAllParcels_WithPagination_ReturnsCorrectPage()
    {
        // Arrange - Create two locker boxes
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        for (int i = 0; i < 3; i++)
        {
            var parcel = new CreateParcelDto
            {
                ParcelSize = "Small",
                InitialLockerId = initialLockerId,
                TargetLockerId = targetLockerId
            };
            var createResponse = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", parcel);
            createResponse.EnsureSuccessStatusCode();
        }

        // Act
        var response = await _client.GetAsync($"{ApiLinks.ParcelsUrl}?page=1&recordsPerPage=2");

        // Assert
        response.EnsureSuccessStatusCode();
        var parcels = await response.Content.ReadFromJsonAsync<List<GetParcelDto>>();
        Assert.NotNull(parcels);
        Assert.True(parcels.Count <= 2);
    }
}