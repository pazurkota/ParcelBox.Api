using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Repositories;

public class LockerRepository : IRepository<Locker>
{
    private readonly List<Locker> _lockers = new();
    
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