using Delegates.Entities;

namespace Delegates.Services;

// Action<Order> — executes a side effect (no return value)
public static class NotificationService
{
    public static readonly Action<Order> LogToConsole =
        order => Console.WriteLine($"  [LOG]   {order}");

    public static readonly Action<Order> SendEmail =
        order => Console.WriteLine($"  [EMAIL] Sending confirmation to {order.Customer}");

    public static readonly Action<Order> SaveAudit =
        order => Console.WriteLine($"  [AUDIT] Order #{order.Id} recorded");
}
