using ParcelBox.Api.Model;

namespace ParcelBox.Api.Dtos.Locker;

public class GetLockersDto
{
    public int Id { get; set; }
    
    public required string Code { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }

    public List<Model.LockerBox> LockerBoxes { get; set; } = [];
}