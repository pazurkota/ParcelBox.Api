using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Parcel;

namespace ParcelBox.Api.Tests.ParcelsTests;

public class EditParcelTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory> 
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly ParcelTestHandler _handler = new(factory);
    
    [Fact]
    public async Task EditParcel_UpdatesTargetLockerAndStatus()
    {
        // Arrange
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        var createResponse = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", newParcel);
        createResponse.EnsureSuccessStatusCode();
        var createdParcel = await createResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        var editDto = new EditParcelDto
        {
            TargetLockerId = targetLockerId,
            ParcelStatus = "OnTheWay"
        };

        // Act
        var editResponse = await _client.PutAsJsonAsync($"{ApiLinks.ParcelsUrl}/{createdParcel!.Id}/edit", editDto);

        // Assert
        editResponse.EnsureSuccessStatusCode();
        var updatedParcelResponse = await _client.GetAsync($"{ApiLinks.ParcelsUrl}/{createdParcel.Id}");
        updatedParcelResponse.EnsureSuccessStatusCode();
        var updatedParcel = await updatedParcelResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        Assert.NotNull(updatedParcel);
        Assert.Equal("OnTheWay", updatedParcel.ParcelStatus);
        Assert.Equal(targetLockerId, updatedParcel.TargetLockerId);
    }

    [Fact]
    public async Task EditParcel_ReturnsNotFoundForInvalidId()
    {
        // Arrange
        var editDto = new EditParcelDto
        {
            TargetLockerId = 9999,
            ParcelStatus = "Delivered"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{ApiLinks.ParcelsUrl}/999999/edit", editDto);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task EditParcel_ReturnsBadRequestForInvalidStatus()
    {
        // Arrange
        var initialLockerId = await _handler.CreateLockerWithBoxesAsync();
        var targetLockerId = await _handler.CreateLockerWithBoxesAsync();

        var newParcel = new CreateParcelDto
        {
            ParcelSize = "Small",
            InitialLockerId = initialLockerId,
            TargetLockerId = targetLockerId
        };

        var createResponse = await _client.PostAsJsonAsync($"{ApiLinks.ParcelsUrl}/create", newParcel);
        createResponse.EnsureSuccessStatusCode();
        var createdParcel = await createResponse.Content.ReadFromJsonAsync<GetParcelDto>();

        var editDto = new EditParcelDto
        {
            ParcelStatus = "InvalidStatus"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{ApiLinks.ParcelsUrl}/{createdParcel!.Id}/edit", editDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}