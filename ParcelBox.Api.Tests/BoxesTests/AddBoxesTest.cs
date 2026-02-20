using System.Net;
using System.Net.Http.Json;
using ParcelBox.Api.Dtos.LockerBox;

namespace ParcelBox.Api.Tests.BoxesTests;

public class AddBoxesTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly BoxTestHandler _handler = new(factory);

    [Fact]
    public async Task AddLockerBoxes_ReturnsOkResult()
    {
        // Arrange
        var lockerId = await _handler.CreateLockerAsync();

        var createLockers = new CreateLockerBoxesDtos
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() { LockerSize = "Small" },
                new() { LockerSize = "Medium" },
                new() { LockerSize = "Big" }
            }
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{ApiLinks.BoxesUrl}/add/{lockerId}", createLockers);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task AddLockerBoxes_ReturnsBadRequest()
    {
        // Arrange
        var lockerId = await _handler.CreateLockerAsync();

        // Act
        var response = await _client.PutAsJsonAsync($"{ApiLinks.BoxesUrl}/add/{lockerId}", new CreateLockerBoxesDtos());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddLockerBoxes_ReturnsNotFound()
    {
        // Arrange
        var createLockers = new CreateLockerBoxesDtos
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() { LockerSize = "Small" },
                new() { LockerSize = "Medium" },
                new() { LockerSize = "Big" }
            }
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{ApiLinks.BoxesUrl}/add/0", createLockers);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
