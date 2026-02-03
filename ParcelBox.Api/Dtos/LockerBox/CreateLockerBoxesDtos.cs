using System.Text.Json.Serialization;

namespace ParcelBox.Api.Dtos.LockerBox;

public class CreateLockerBoxesDtos
{
    [JsonPropertyName("lockerBoxes")]
    public List<CreateLockerBoxDto> BoxDtos { get; set; } = [];
}