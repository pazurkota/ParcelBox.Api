using System.Collections.Generic;

namespace ParcelBox.Api.Abstraction;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}