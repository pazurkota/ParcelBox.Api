namespace ParcelBox.Api.Model;

public class Parcel
{
    public int Id { get; set; }
    
    public required string PickupCode { get; set; }
    public Size ParcelSize { get; set; }
    
    public int InitialLockerId { get; set; }
    public int TargetLockerId { get; set; }
    
    public int InitialLockerBoxId { get; set; }
    public int TargetLockerBoxId { get; set; }
}