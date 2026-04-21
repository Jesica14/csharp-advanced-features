using AsyncAwait.Entities;

namespace AsyncAwait.Services;

public class OrderService
{
    // Simulate a DB save — I/O bound operation
    public async Task SaveOrderAsync(Order order)
    {
        await Task.Delay(200); // simulates DB latency
        Console.WriteLine($"  [DB] Order #{order.Id} saved");
    }

    // Simulate fetching an order from DB
    public async Task<Order> GetOrderAsync(int id)
    {
        await Task.Delay(80);
        return new Order(id, $"Customer_{id}", id * 100m);
    }

    // Simulate sending an email — I/O bound
    public async Task SendEmailAsync(string customer)
    {
        await Task.Delay(50);
        Console.WriteLine($"  [EMAIL] Confirmation sent to {customer}");
    }

    // Simulate a flaky external API that sometimes fails
    public async Task<string> GetExternalDataAsync(bool shouldFail = false)
    {
        await Task.Delay(50);
        if (shouldFail)
        {
            throw new HttpRequestException("External API is unavailable");
        }
        return "{ \"status\": \"ok\" }";
    }

    // CPU-bound — heavy calculation (not I/O)
    public int CalculateTotal(IEnumerable<Order> orders) 
    {
        return orders.Sum(o => (int)o.Amount);
    }
}
