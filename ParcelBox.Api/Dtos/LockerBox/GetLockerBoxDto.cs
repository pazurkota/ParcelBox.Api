using ParcelBox.Api.Model;

namespace ParcelBox.Api.Dtos.LockerBox;

public class GetLockerBoxDto
{
    public int Id { get; set; }
    public Size LockerSize { get; set; }
    public bool IsOccupied { get; set; }
    public int LockerId { get; set; }
}