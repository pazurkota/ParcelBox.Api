namespace ParcelBox.Api.Dtos.Locker;

public class CreateLockerDto
{
    public required string Code { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
}