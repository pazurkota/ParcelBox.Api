using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Controllers;

/// <inheritdoc />
public class LockersController(IRepository<Locker> repository) 
    : BaseController
{
    /// <summary>
    /// Get all lockers
    /// </summary>
    /// <returns>An array of lockers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetLockerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetAllLockers()
    {
        var lockers = repository.GetAll();
        
        return Ok(lockers.Select(locker => new GetLockerDto
        {
            Id = locker.Id,
            Code = locker.Code,
            Address = locker.Address,
            City = locker.City,
            LockerBoxes = locker.LockerBoxes,
            PostalCode = locker.PostalCode
        }));
    }

    
    /// <summary>
    /// Gets a locker by ID
    /// </summary>
    /// <param name="id">The ID of a locker</param>
    /// <returns>A single locker record</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetLockerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    
    
    /// <summary>
    /// Creates a new locker
    /// </summary>
    /// <param name="lockerDto">The locker to be created</param>
    /// <returns>A link to the locker that was created</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(GetLockerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    
    /// <summary>
    /// Creates a locker boxes in a locker
    /// </summary>
    /// <param name="id">The ID of a locker</param>
    /// <param name="lockerBoxDtos">An array of locker boxes to be created</param>
    /// <returns></returns>
    [HttpPatch("{id:int}/boxes")]
    [ProducesResponseType(typeof(GetLockerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    
    /// <summary>
    /// Edits the locker
    /// </summary>
    /// <param name="id">The ID of a locker</param>
    /// <param name="lockerDto">The data to be edited</param>
    /// <returns></returns>
    [HttpPut("{id:int}/edit")]
    public IActionResult EditLocker(int id, [FromBody] EditLockerDto lockerDto)
    {
        var existingLocker = repository.GetById(id);
        if (existingLocker == null) return NotFound();

        existingLocker.Address = lockerDto.Address;

        return Ok();
    }
}