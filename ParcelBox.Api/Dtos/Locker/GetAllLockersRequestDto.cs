namespace ParcelBox.Api.Dtos.Locker;

public class GetAllLockersRequestDto
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    
    public string? LockerCode { get; set; }
    public string? LockersAreInCity { get; set; }
}