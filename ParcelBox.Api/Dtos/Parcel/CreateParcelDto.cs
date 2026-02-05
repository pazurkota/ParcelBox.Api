namespace ParcelBox.Api.Dtos.Parcel;

public class CreateParcelDto
{
    public required string PickupCode { get; set; }
    public required string ParcelSize { get; set; }
    
    public int InitialLockerId { get; set; }
    public int TargetLockerId { get; set; }
}