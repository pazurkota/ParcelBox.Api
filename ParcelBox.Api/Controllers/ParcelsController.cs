using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Database;
using ParcelBox.Api.Dtos.Parcel;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Controllers;

public class ParcelsController(AppDbContext dbContext) : BaseController
{
    public async Task<IActionResult> GetAllParcels([FromQuery] GetAllParcelsRequestDto requestDto)
    {
        int page = requestDto?.Page ?? 1;
        int numberOfRecords = requestDto?.RecordsPerPage ?? 100;

        var query = dbContext.Parcels
            .Skip((page - 1) * numberOfRecords)
            .Take(numberOfRecords);

        if (!string.IsNullOrWhiteSpace(requestDto.Size))
        {
            if (!Enum.TryParse<Size>(requestDto.Size, ignoreCase: true, out var size))
            {
                return BadRequest($"Invalid locker size value: '{requestDto.Size}'.");
            }

            query = query.Where(x => x.ParcelSize == size);
        }
        
        var parcels = await query.ToArrayAsync();

        return Ok(parcels.Select(ParcelMapper.ParcelToGetParcelDto));
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetParcelById(int id)
    {
        var parcel = await dbContext.Parcels.SingleOrDefaultAsync(x => x.Id == id);
        if (parcel is null) return NotFound();

        var existingParcel = ParcelMapper.ParcelToGetParcelDto(parcel);
        return Ok(existingParcel);
    }
}