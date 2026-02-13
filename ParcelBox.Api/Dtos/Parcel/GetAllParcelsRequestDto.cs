namespace ParcelBox.Api.Dtos.Parcel;

public class GetAllParcelsRequestDto
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    
    public string? Size { get; set; }
}