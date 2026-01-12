using Microsoft.AspNetCore.Mvc;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Controllers;

public class LockerController(IRepository<Locker> repository) 
    : BaseController
{
    [HttpGet]
    public IActionResult GetAllLockers()
    {
        var lockers = repository.GetAll();
        
        return Ok(lockers.Select(locker => new GetLockerResponse
        {
            Code = locker.Code,
            Address = locker.Address,
            City = locker.City,
            LockerBoxes = locker.LockerBoxes
        }));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetLockerById(int id)
    {
        var locker = repository.GetById(id);

        if (locker == null) return NotFound();

        return Ok(new GetLockerResponse
        {
            Code = locker.Code,
            Address = locker.Address,
            City = locker.Code,
            LockerBoxes = locker.LockerBoxes
        });
    }
}