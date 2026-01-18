namespace ParcelBox.Api.Dtos.Locker;

public class CreateLockerDto
{
    public string Code { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
}