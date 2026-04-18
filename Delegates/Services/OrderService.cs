using Delegates.Entities;

namespace Delegates.Services;

// event — a multicast delegate with access control (only OrderService can invoke OnOrderCreated
public class OrderService
{
    public event Action<Order>? OnOrderCreated;

    public Order Create(int id, string customer, decimal amount, bool isPaid = false)
    {
        var order = new Order { Id = id, Customer = customer, Amount = amount, IsPaid = isPaid };

        OnOrderCreated?.Invoke(order); // notify all subscribers

        return order;
    }
}
