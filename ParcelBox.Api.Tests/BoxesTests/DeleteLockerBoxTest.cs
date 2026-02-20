using System.Net;

namespace ParcelBox.Api.Tests.BoxesTests;

public class DeleteLockerBoxTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly BoxTestHandler _handler = new(factory);

    [Fact]
    public async Task DeleteLockerBox_RemovesBoxAndReturnsNoContent()
    {
        // Arrange
        var (_, boxes) = await _handler.CreateLockerWithBoxesAsync();
        var boxId = boxes[0].Id;

        // Act
        var deleteResponse = await _client.DeleteAsync($"{ApiLinks.BoxesUrl}/{boxId}/delete");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"{ApiLinks.BoxesUrl}/{boxId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteLockerBox_ReturnsNotFound_WhenBoxDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync($"{ApiLinks.BoxesUrl}/999999/delete");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
