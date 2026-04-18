using Generics.Entities;

namespace Generics.Services;

// where T : IValidator — guarantees IsValid() and ValidationError() on any entity
public static class Validator
{
    public static void Validate<T>(T entity) where T : IValidator
    {
        if (!entity.IsValid())
        {
            throw new InvalidOperationException(entity.ValidationError());
        }
    }
}
