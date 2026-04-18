namespace Generics.Services;

// where T : struct — T? becomes Nullable<T>, so .HasValue is always safe
public static class NullableHelper
{
    public static T Unwrap<T>(T? value, T fallback) where T : struct 
    {
        return value ?? fallback;
    }
}
