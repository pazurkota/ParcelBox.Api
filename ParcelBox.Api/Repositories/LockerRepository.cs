using System;
using System.Collections.Generic;
using System.Linq;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Repositories;

public class LockerRepository : IRepository<Locker>
{
    // yup, I used pentagon address because why not?
    private readonly List<Locker> _lockers = new() 
    {
        new Locker
        {
            Id = 1,
            Address = "1400 Defense Pentagon",
            City = "Washington DC",
            PostalCode = "20301 USA",
            Code = "WAS-001",
            LockerBoxes = 
            [
                new LockerBox
                {
                    Id = 1,
                    LockerSize = Size.Small,
                    IsOccupied = false,
                    LockerId = 1
                },
                
                new LockerBox
                {
                    Id = 2,
                    LockerSize = Size.Medium,
                    IsOccupied = true,
                    LockerId = 1
                },
                
                new LockerBox
                {
                    Id = 3,
                    LockerSize = Size.Big,
                    IsOccupied = false,
                    LockerId = 1
                }
            ]
        }
    };
    
    public IEnumerable<Locker> GetAll()
    {
        return _lockers;
    }

    public Locker? GetById(int id)
    {
        return _lockers.FirstOrDefault(x => x.Id == id);
    }

    public void Create(Locker entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.Id = _lockers.Select(e => e.Id).DefaultIfEmpty(0).Max() + 1;
        _lockers.Add(entity);
    }

    public void Update(Locker entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var locker = _lockers.FirstOrDefault(x => x.Id == entity.Id);
        locker?.LockerBoxes = entity.LockerBoxes;
    }

    public void Delete(Locker entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _lockers.Remove(entity);
    }
}