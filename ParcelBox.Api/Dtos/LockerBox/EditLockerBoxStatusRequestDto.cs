using System.Text.Json.Serialization;

namespace ParcelBox.Api.Dtos.LockerBox;

public class EditLockerBoxStatusRequestDto
{
    public int? BoxId { get; set; }
    public bool IsOccupied { get; set; }
}