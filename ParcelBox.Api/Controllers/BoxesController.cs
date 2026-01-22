using System;
using Microsoft.AspNetCore.Mvc;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Controllers;

public class BoxesController(IRepository<Locker> repository) : BaseController
{
    [HttpPut("add/{lockerId:int}")]
    public IActionResult AddLockerBoxes(int lockerId, [FromBody] CreateLockerBoxDto[] lockerBoxDtos)
    {
        var existingLocker = repository.GetById(lockerId);
        if (existingLocker == null) return NotFound();

        foreach (var boxDto in lockerBoxDtos)
        {
            if (!Enum.TryParse<Size>(boxDto.LockerSize, ignoreCase: true, out var size))
            {
                // this is a temp solution (for now)
                return BadRequest($"Invalid locker box size: '{boxDto.LockerSize}'");
            }
            
            existingLocker.LockerBoxes.Add(new LockerBox
            {
                Id = existingLocker.LockerBoxes.Count + 1,
                IsOccupied = false,
                LockerId = existingLocker.Id,
                LockerSize = size
            });
        }

        return Ok();
    }

    [HttpPatch("occupied/{lockerId:int}/{boxId:int}/{isOccupied:bool}")]
    public IActionResult EditLockerBoxStatus(int lockerId, int boxId, bool isOccupied)
    {
        var existingLocker = repository.GetById(lockerId);
        if (existingLocker == null) return NotFound();

        var existingBox = existingLocker.LockerBoxes.FirstOrDefault(x => x.Id == boxId);
        if (existingBox == null) return NotFound();

        return Ok(existingBox.IsOccupied = isOccupied);
    }
}