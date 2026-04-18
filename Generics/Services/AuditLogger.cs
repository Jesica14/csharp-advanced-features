using Generics.Entities;

namespace Generics.Services;

// where T : BaseEntity — guarantees access to ToLog(), works for any entity
public static class AuditLogger
{
    public static void Log<T>(T entity) where T : BaseEntity 
    {
        Console.WriteLine($"  AUDIT | {entity.ToLog()}");
    }
}
