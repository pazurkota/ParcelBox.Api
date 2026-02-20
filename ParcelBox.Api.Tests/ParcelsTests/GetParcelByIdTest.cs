using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Parcel;

namespace ParcelBox.Api.Tests.ParcelsTests;

public class GetParcelByIdTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly ParcelTestHandler _handler = new(factory);
    
    [Fact]
    public async Task GetParcelById_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{ApiLinks.ParcelsUrl}/999999");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetParcelById_AfterCreate_ReturnsOkResult()
    {
        // Arrange
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Medium",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        var createResponse = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", newParcel);
        createResponse.EnsureSuccessStatusCode();
        var createdParcel = await createResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        // Act
        var response = await _client.GetAsync($"{ApiLinks.ParcelsUrl}/{createdParcel!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var parcel = await response.Content.ReadFromJsonAsync<GetParcelDto>();
        Assert.NotNull(parcel);
        Assert.Equal(createdParcel.Id, parcel.Id);
        Assert.Equal("Medium", parcel.ParcelSize);
    }
}