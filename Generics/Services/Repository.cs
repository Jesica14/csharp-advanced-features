using Generics.Entities;

namespace Generics.Services;

// where T : class
// Needs reference semantics — entities live on the heap and are shared by reference.
public class Repository<T> where T : class
{
    private readonly List<T> _store = [];

    public void Add(T item) => _store.Add(item);
    
    public IEnumerable<T> GetAll() => _store.AsReadOnly();
}