namespace ParcelBox.Api.Dtos.LockerBox;

public class GetLockerResponse
{
    public required string Code { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }

    public Model.LockerBox[] LockerBoxes { get; set; } = [];
}