using Delegates.Entities;
using Delegates.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        // Custom delegate (swap behavior at runtime)
        LogFormatter formatter = LogFormatterExamples.Timestamp;
        Console.WriteLine(formatter("Order system started"));

        formatter = LogFormatterExamples.Uppercase;
        Console.WriteLine(formatter("Order system started"));


        // Func
        decimal basePrice = 1000m;

        var withTax = PricingService.Apply(basePrice, PricingService.WithTax);
        var withDiscount = PricingService.Apply(basePrice, PricingService.WithDiscount);
        var custom = PricingService.Apply(basePrice, p => p * 2);


        // Action
        Action<Order> notify = NotificationService.LogToConsole;
        notify += NotificationService.SendEmail;
        notify += NotificationService.SaveAudit;

        notify += o => Console.WriteLine($"[CUSTOM] Order processed: {o.Id}");

        var order = new Order
        {
            Id = 1,
            Customer = "Jesica",
            Amount = 1500m,
            IsPaid = true
        };

        notify(order);


        var orders = new List<Order>
        {
            new() { Id = 1, Customer = "Jesica", Amount = 1500m, IsPaid = true },
            new() { Id = 2, Customer = "Ana", Amount = 200m, IsPaid = false },
            new() { Id = 3, Customer = "Luis", Amount = 3000m, IsPaid = true }
        };

        // Predicate
        var paidOrders = OrderFilter.Filter(orders, o => o.IsPaid);
        var largeOrders = OrderFilter.Filter(orders, o => o.Amount > 1000);
        var paidOrdersLinq = orders.Where(o => o.IsPaid).ToList();


        // Event
        var service = new OrderService();

        service.OnOrderCreated += o => Console.WriteLine($"[EVENT] Order created: {o.Id}");
        service.OnOrderCreated += NotificationService.SendEmail;
        service.Create(id: 10, customer: "Jesica", amount: 2500m);
    }
}