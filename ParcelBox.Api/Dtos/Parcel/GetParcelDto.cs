namespace ParcelBox.Api.Dtos.Parcel;

public class GetParcelDto
{
    public int Id { get; set; }
    
    public required string PickupCode { get; set; }
    public required string ParcelSize { get; set; }
    public required string ParcelStatus { get; set; }
    
    public int InitialLockerId { get; set; }
    public int TargetLockerId { get; set; }
    
    public int InitialLockerBoxId { get; set; }
    public int TargetLockerBoxId { get; set; }
}