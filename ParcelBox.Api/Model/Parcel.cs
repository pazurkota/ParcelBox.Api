namespace ParcelBox.Api.Model;

public class Parcel
{
    public int Id { get; set; }
    public required string PickupCode { get; set; }
    
    public int InitialParcelId { get; set; }
    public int TargetParcelId { get; set; }
}