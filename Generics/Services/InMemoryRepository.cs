using Generics.Entities;

namespace Generics.Services;

// where T : BaseEntity — needs Id to implement GetById
public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly List<T> _data = [];

    public void Add(T entity)
    {
        _data.Add(entity);
    }
    
    public T? GetById(int id)
    {
        return _data.FirstOrDefault(x => x.Id == id);
    }
}
