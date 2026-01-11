namespace ParcelBox.Api.Model;

public class Parcel
{
    public int Id { get; set; }
    public required string PickupCode { get; set; }
    
    public enum Size
    {
        Small,
        Medium,
        Big
    }

    public enum Status
    {
        Sent,
        OnItsWay,
        InLocker,
        Received
    }
    
    public int InitialParcelId { get; set; }
    public int TargetParcelId { get; set; }
}