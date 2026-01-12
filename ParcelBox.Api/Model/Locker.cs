namespace ParcelBox.Api.Model;

public class Locker
{
    public int Id { get; set; }
    
    public required string Code { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }

    public LockerBox[] LockerBoxes { get; set; } = [];
}