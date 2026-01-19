namespace ParcelBox.Api.Model;

public class LockerBox
{
    public int Id { get; set; }
 
    public required string LockerSize { get; set; }
    
    public bool IsOccupied { get; set; }
    public int LockerId { get; set; }
}