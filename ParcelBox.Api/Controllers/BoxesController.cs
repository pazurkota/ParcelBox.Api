using System;
using Microsoft.AspNetCore.Mvc;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Database;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Controllers;

public class BoxesController(AppDbContext dbContext) : BaseController
{
    [HttpPut("add/{lockerId:int}")]
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

        return Ok();
    }
/*
    [HttpPatch("occupied/{lockerId:int}/{boxId:int}/{isOccupied:bool}")]
    public IActionResult EditLockerBoxStatus(int lockerId, int boxId, bool isOccupied)
    {
        var existingLocker = repository.GetById(lockerId);
        if (existingLocker == null) return NotFound();

        var existingBox = existingLocker.LockerBoxes.FirstOrDefault(x => x.Id == boxId);
        if (existingBox == null) return NotFound();

        return Ok(existingBox.IsOccupied = isOccupied);
    }
*/
}