using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Database;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Controllers;

/// <inheritdoc />
public class LockersController(AppDbContext dbContext) 
    : BaseController
{
    /// <summary>
    /// Get all lockers
    /// </summary>
    /// <returns>An array of lockers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetLockersDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllLockers([FromQuery] GetAllLockersRequestDto requestDto)
    {
        int page = requestDto?.Page ?? 1;
        int numberOfRecords = requestDto?.RecordsPerPage ?? 100;

        var query = dbContext.Lockers
            .Include(x => x.LockerBoxes)
            .Skip((page - 1) * numberOfRecords)
            .Take(numberOfRecords);

        if (requestDto is not null)
        {
            if (!string.IsNullOrWhiteSpace(requestDto.LockerCode))
            {
                query = query.Where(x => x.PostalCode.Contains(requestDto.LockerCode));
            }

            if (!string.IsNullOrWhiteSpace(requestDto.LockersAreInCity))
            {
                query = query.Where(x => x.City.Contains(requestDto.LockersAreInCity));
            }
        }
        
        var lockers = await query.ToArrayAsync();
        
        return Ok(lockers.Select(LockersToGetLockersRequestDto));
    }

 
    /// <summary>
    /// Gets a locker by ID
    /// </summary>
    /// <param name="id">The ID of a locker</param>
    /// <returns>A single locker record</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetSingleLockerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLockerById(int id)
    {
        var locker = await dbContext.Lockers
            .Include(x => x.LockerBoxes)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (locker == null) return NotFound();

        var existingLocker = LockersToGetLockersRequestDto(locker);
        return Ok(existingLocker);
    }


    /// <summary>
    /// Creates a new locker
    /// </summary>
    /// <param name="lockerDto">The locker to be created</param>
    /// <returns>A link to the locker that was created</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(GetLockersDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateLocker([FromBody] CreateLockerDto lockerDto)
    {
        Locker newLocker = new()
        {
            Code = lockerDto.Code,
            Address = lockerDto.Address,
            City = lockerDto.City,
            PostalCode = lockerDto.PostalCode
        };

        dbContext.Lockers.Add(newLocker);
        await dbContext.SaveChangesAsync();
        
        var resultDto = new GetLockersDto 
        {
            Id = newLocker.Id, 
            Code = newLocker.Code,
            Address = newLocker.Address,
            City = newLocker.City,
            PostalCode = newLocker.PostalCode
        };
        
        return Created($"/locker/{newLocker.Id}", resultDto);
    }

    /// <summary>
    /// Edits the locker
    /// </summary>
    /// <param name="id">The ID of a locker</param>
    /// <param name="lockerDto">The data to be edited</param>
    /// <returns></returns>
    [HttpPut("{id:int}/edit")]
    public async Task<IActionResult> EditLocker(int id, [FromBody] EditLockerDto lockerDto)
    {
        var existingLocker = await dbContext.Lockers.FindAsync(id);
        if (existingLocker == null) return NotFound();

        existingLocker.Address = lockerDto.Address;

        dbContext.Entry(existingLocker).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        
        return Ok();
    }

    /// <summary>
    /// Deletes the locker
    /// </summary>
    /// <param name="id">The ID of a locker</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteLocker(int id)
    {
        var locker = await dbContext.Lockers.FindAsync(id);

        if (locker is null) return NotFound();

        dbContext.Lockers.Remove(locker);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    private static GetLockersDto LockersToGetLockersRequestDto(Locker locker)
    {
        return new GetLockersDto
        {
            Id = locker.Id,
            Code = locker.Code,
            Address = locker.Address,
            City = locker.City,
            PostalCode = locker.PostalCode
        };
    }
}