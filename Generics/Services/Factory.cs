namespace Generics.Services;

// where T : new() — allows calling new T() without knowing the concrete type
public static class Factory
{
    public static T Create<T>() where T : new()
    {
        return new T();
    }
}
