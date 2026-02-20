using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.Parcel;

namespace ParcelBox.Api.Tests.ParcelsTests;

public class DeleteParcelTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory> 
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly ParcelTestHandler _handler = new(factory);
    
    [Fact]
    public async Task DeleteParcel_RemovesParcelAndReturnsNoContent()
    {
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

        var deleteResponse = await _client.DeleteAsync($"{ApiLinks.ParcelsUrl}/{createdParcel!.Id}/delete");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"{ApiLinks.ParcelsUrl}/{createdParcel.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteParcel_ReturnsNotFoundForNonExistentParcel()
    {
        var response = await _client.DeleteAsync($"{ApiLinks.ParcelsUrl}/999999/delete");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}