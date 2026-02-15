using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Database;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Controllers;

public class BoxesController(AppDbContext dbContext) : BaseController
{
    /// <summary>
    /// Adds a locker boxes into locker
    /// </summary>
    /// <param name="lockerId">The ID of a locker</param>
    /// <param name="createLockerBoxesDtos"></param>
    /// <returns></returns>
    [HttpPut("add/{lockerId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddLockerBoxes(int lockerId, [FromBody] CreateLockerBoxesDtos createLockerBoxesDtos)
    {
        // return 404 if locker not found
        var isLockerExisting = dbContext.Lockers.Any(x => x.Id == lockerId);
        if (!isLockerExisting) return NotFound();

        List<LockerBox> lockerBoxes = new();
        
        foreach (var boxDto in createLockerBoxesDtos.BoxDtos)
        {
            if (!Enum.TryParse<Size>(boxDto.LockerSize, ignoreCase: true, out var size))
            {
                return BadRequest($"Invalid locker size value: '{boxDto.LockerSize}'.");
            }
            
            lockerBoxes.Add(new LockerBox
            {
                IsOccupied = false,
                LockerId = lockerId,
                LockerSize = size
            });
        }
        
        dbContext.LockerBoxes.AddRange(lockerBoxes);
        await dbContext.SaveChangesAsync();

        return Ok(lockerBoxes);
    }

    
    /// <summary>
    /// Edits the status of a locker
    /// </summary>
    /// <param name="requestDto"></param>
    /// <returns></returns>
    [HttpPatch("occupied")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditLockerBoxStatus([FromQuery] EditLockerBoxStatusRequestDto requestDto)
    {
        var existingLockerBox = await dbContext.LockerBoxes
            .FirstOrDefaultAsync(x => x.Id == requestDto.BoxId);
        
        if (existingLockerBox is null) return NotFound();

        existingLockerBox.IsOccupied = requestDto.IsOccupied;
        
        dbContext.Entry(existingLockerBox).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        
        return Ok();
    }

    
    /// <summary>
    /// Deletes the locker box
    /// </summary>
    /// <param name="id">The ID of a locker</param>
    /// <returns></returns>
    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteLockerBox(int id)
    {
        var existingLockerBox = await dbContext.LockerBoxes.FindAsync(id);
        if (existingLockerBox is null) return NotFound();

        dbContext.LockerBoxes.Remove(existingLockerBox);
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }
}