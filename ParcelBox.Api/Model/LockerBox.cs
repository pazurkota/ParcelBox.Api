namespace ParcelBox.Api.Model;

public class LockerBox
{
    public int Id { get; set; }
    
    public enum Size
    {
        Small,
        Medium,
        Big
    }
    
    public bool IsOccupied { get; set; }
    public int LockerId { get; set; }
}