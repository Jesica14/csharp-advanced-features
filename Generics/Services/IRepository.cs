using Generics.Entities;

namespace Generics.Services;

// where T : class — reference semantics needed to store and share entities
public interface IRepository<T> where T : class
{
    void Add(T entity);
    
    T? GetById(int id);
}
