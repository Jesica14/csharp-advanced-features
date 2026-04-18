using Delegates.Entities;

namespace Delegates.Services;

// Predicate<Order> — returns bool, used to filter collections
public static class OrderFilter
{
    public static readonly Predicate<Order> IsPaid = o => o.IsPaid;
    
    public static readonly Predicate<Order> IsLarge = o => o.Amount > 1000;

    public static List<Order> Filter(List<Order> orders, Predicate<Order> match) 
    {
        return orders.FindAll(match);
    }
}
