namespace Delegates.Services;

// Custom delegate — defines the signature a log formatter must follows
public delegate string LogFormatter(string message);

public static class LogFormatterExamples
{
    public static string Timestamp(string message) 
    {
        return $"[{DateTime.Now:HH:mm:ss}] {message}";
    }

    public static string Uppercase(string message) 
    {
        return message.ToUpper();
    }
}
