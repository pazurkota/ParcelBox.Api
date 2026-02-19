using System.Net;
using System.Net.Http.Json;

namespace ParcelBox.Api.Tests.BoxesTests;

public class EditLockerBoxStatusTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly BoxTestHandler _handler = new(factory);

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsOkResult_WhenSettingOccupiedToTrue()
    {
        // Arrange
        var (_, boxes) = await _handler.CreateLockerWithBoxesAsync();
        var boxIdToEdit = boxes[0].Id;

        // Act
        var response = await _client
            .PatchAsJsonAsync($"{ApiLinks.BoxesUrl}/occupied?boxId={boxIdToEdit}&isOccupied=true", "");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsOkResult_WhenSettingOccupiedToFalse()
    {
        // Arrange
        var (_, boxes) = await _handler.CreateLockerWithBoxesAsync();
        var boxIdToEdit = boxes[0].Id;

        // First set to occupied
        await _client.PatchAsJsonAsync($"{ApiLinks.BoxesUrl}/occupied?boxId={boxIdToEdit}&isOccupied=true", "");

        // Act - set back to not occupied
        var response = await _client
            .PatchAsJsonAsync($"{ApiLinks.BoxesUrl}/occupied?boxId={boxIdToEdit}&isOccupied=false", "");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsNotFound_WhenBoxIdDoesNotExist()
    {
        // Arrange
        var nonExistentBoxId = 99999;

        // Act
        var response = await _client
            .PatchAsJsonAsync($"{ApiLinks.BoxesUrl}/occupied?boxId={nonExistentBoxId}&isOccupied=true", "");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsBadRequest_WhenBoxIdIsMissing()
    {
        // Act
        var response = await _client
            .PatchAsJsonAsync($"{ApiLinks.BoxesUrl}/occupied?isOccupied=true", "");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsBadRequest_WhenBoxIdIsZero()
    {
        // Act
        var response = await _client
            .PatchAsJsonAsync($"{ApiLinks.BoxesUrl}/occupied?boxId=0&isOccupied=true", "");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EditLockerBoxStatus_ReturnsBadRequest_WhenBoxIdIsNegative()
    {
        // Act
        var response = await _client
            .PatchAsJsonAsync($"{ApiLinks.BoxesUrl}/occupied?boxId=-1&isOccupied=true", "");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
