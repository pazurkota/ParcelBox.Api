using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Controllers;

public class LockersController(IRepository<Locker> repository) 
    : BaseController
{
    [HttpGet]
    public IActionResult GetAllLockers()
    {
        var lockers = repository.GetAll();
        
        return Ok(lockers.Select(locker => new GetLockerDto
        {
            Code = locker.Code,
            Address = locker.Address,
            City = locker.City,
            LockerBoxes = locker.LockerBoxes,
            PostalCode = locker.PostalCode
        }));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetLockerById(int id)
    {
        var locker = repository.GetById(id);

        if (locker == null) return NotFound();

        return Ok(new GetLockerDto
        {
            Code = locker.Code,
            Address = locker.Address,
            City = locker.Code,
            LockerBoxes = locker.LockerBoxes,
            PostalCode = locker.PostalCode
        });
    }

    [HttpPost("create")]
    public IActionResult CreateLocker([FromBody] CreateLockerDto lockerDto)
    {
        Locker newLocker = new()
        {
            Code = lockerDto.Code,
            Address = lockerDto.Address,
            City = lockerDto.City,
            PostalCode = lockerDto.PostalCode
        };
        
        repository.Create(newLocker);
        return Created($"/locker/{newLocker.Id}", lockerDto);
    }

    [HttpPatch("{id:int}/boxes")]
    public IActionResult CreateLockerBoxes(int id, [FromBody] CreateLockerBoxDto[] lockerBoxDtos)
    {
        var existingLocker = repository.GetById(id);
        if (existingLocker == null) return NotFound();

        foreach (var boxDto in lockerBoxDtos)
        {
            existingLocker.LockerBoxes.Add(new LockerBox()
            {
                Id = existingLocker.LockerBoxes.Count + 1,
                IsOccupied = false,
                LockerId = existingLocker.Id,
                LockerSize = boxDto.LockerSize
            });
        }

        return Created($"/locker/{existingLocker.Id}", lockerBoxDtos);
    }
}