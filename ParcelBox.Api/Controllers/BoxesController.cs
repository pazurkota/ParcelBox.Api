using Microsoft.AspNetCore.Mvc;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Dtos.Locker;
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
            existingLocker.LockerBoxes.Add(new LockerBox
            {
                Id = existingLocker.LockerBoxes.Count + 1,
                IsOccupied = false,
                LockerId = existingLocker.Id,
                LockerSize = boxDto.LockerSize
            });
        }

        return Ok();
    }
}