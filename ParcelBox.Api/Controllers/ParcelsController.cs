using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Database;
using ParcelBox.Api.Dtos.Parcel;
using ParcelBox.Api.Mappers;
using ParcelBox.Api.Model;
using ParcelBox.Api.Services;

namespace ParcelBox.Api.Controllers;

public class ParcelsController(AppDbContext dbContext) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllParcels([FromQuery] GetAllParcelsRequestDto requestDto)
    {
        int page = requestDto?.Page ?? 1;
        int numberOfRecords = requestDto?.RecordsPerPage ?? 100;

        var query = dbContext.Parcels
            .OrderBy(x => x.Id)
            .Skip((page - 1) * numberOfRecords)
            .Take(numberOfRecords);

        if (!string.IsNullOrWhiteSpace(requestDto.Size))
        {
            if (!Enum.TryParse<Size>(requestDto.Size, ignoreCase: true, out var size))
            {
                return BadRequest($"Invalid parcel size value: '{requestDto.Size}'.");
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
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateParcel([FromBody] CreateParcelDto createDto)
    {
        var lockerBoxesIds = await ParcelService
                .SetLockerBoxesAsync(dbContext, createDto.InitialLockerId, createDto.TargetLockerId);

        if (!Enum.TryParse<Size>(createDto.ParcelSize, ignoreCase: true, out var size))
        {
            return BadRequest($"Invalid parcel size value: '{createDto.ParcelSize}'.");
        }

        if (lockerBoxesIds.initialLockerBoxId is null)
        {
            return BadRequest("Couldn't find any available locker boxes in initial locker");
        }
        
        if (lockerBoxesIds.targetLockerBoxId is null)
        {
            return BadRequest("Couldn't find any available locker boxes in target locker");
        }
        
        Parcel newParcel = new()
        {
            PickupCode = ParcelService.GenerateParcelCode(),
            ParcelSize = size,
            ParcelStatus = Status.Send,
            
            InitialLockerId = createDto.InitialLockerId,
            TargetLockerId = createDto.TargetLockerId,
            
            InitialLockerBoxId = lockerBoxesIds.initialLockerBoxId.Value,
            TargetLockerBoxId = lockerBoxesIds.targetLockerBoxId.Value
        };

        await ParcelService.ChangeLockerBoxStatusAsync(dbContext, lockerBoxesIds.initialLockerBoxId.Value, true);
        await ParcelService.ChangeLockerBoxStatusAsync(dbContext, lockerBoxesIds.targetLockerBoxId.Value, true);
        
        dbContext.Parcels.Add(newParcel);
        await dbContext.SaveChangesAsync();

        GetParcelDto parcelDto = new()
        {
            Id = newParcel.Id,
            
            PickupCode = newParcel.PickupCode,
            ParcelSize = newParcel.ParcelSize.ToString(),
            ParcelStatus = newParcel.ParcelStatus.ToString(),
            
            InitialLockerId = newParcel.InitialLockerId,
            TargetLockerId = newParcel.TargetLockerId,
            
            InitialLockerBoxId = newParcel.InitialLockerBoxId,
            TargetLockerBoxId = newParcel.TargetLockerBoxId
        };
        
        return Created($"/parcels/{newParcel.Id}", parcelDto);
    }
    
    [HttpPut("{id:int}/edit")]
    public async Task<IActionResult> EditParcel(int id, [FromBody] EditParcelDto parcelDto)
    {
        var existingParcel = await dbContext.Parcels.FindAsync(id);
        if (existingParcel is null) return NotFound();

        var oldTargetLockerBoxId = existingParcel.TargetLockerBoxId;
        
        if (parcelDto.TargetLockerId is not null)
        {
            var newLockerBoxId =
                await ParcelService.SetNewTargetLockerBoxAsync(dbContext, parcelDto.TargetLockerId.Value);

            existingParcel.TargetLockerId = parcelDto.TargetLockerId.Value;
            existingParcel.TargetLockerBoxId = newLockerBoxId!.Value;

            await ParcelService.ChangeLockerBoxStatusAsync(dbContext, newLockerBoxId.Value, true);
            await ParcelService.ChangeLockerBoxStatusAsync(dbContext, oldTargetLockerBoxId, false);
            
            dbContext.Entry(existingParcel).State = EntityState.Modified;
        }

        if (parcelDto.ParcelStatus is not null)
        {
            if (!Enum.TryParse<Status>(parcelDto.ParcelStatus, ignoreCase: true, out var status))
            {
                return BadRequest($"Invalid parcel status value: '{parcelDto.ParcelStatus}'.");
            }

            existingParcel.ParcelStatus = status;
            dbContext.Entry(existingParcel).State = EntityState.Modified;
        }
        
        await dbContext.SaveChangesAsync();
        
        return Ok();
    }
}