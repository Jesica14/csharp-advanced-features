using AsyncAwait.Entities;
using AsyncAwait.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var service = new OrderService();

        // Exception handling
        try
        {
            await service.GetExternalDataAsync(shouldFail: true);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Handled error: {ex.Message}");
        }


        // Unawaited task (race condition)
        Console.WriteLine("Sending email before save completes (race condition demo)");
        var saveTask = service.SaveOrderAsync(new Order(1, "Jesica", 1000m));
        await service.SendEmailAsync("Jesica");

        await saveTask;


        // Concurrency (WhenAll)
        var t1 = service.GetOrderAsync(1);
        var t2 = service.GetOrderAsync(2);
        var t3 = service.GetOrderAsync(3);

        await Task.WhenAll(t1, t2, t3);


        // Sequential (contrast)
        await service.GetOrderAsync(4);
        await service.GetOrderAsync(5);
        await service.GetOrderAsync(6);


        // Task.WhenAny (timeout pattern)
        var fetchTask = service.GetOrderAsync(10);
        var timeoutTask = Task.Delay(50);

        var winner = await Task.WhenAny(fetchTask, timeoutTask);

        if (winner == fetchTask)
            Console.WriteLine($"Order #{(await fetchTask).Id} received");
        else
            Console.WriteLine("Timeout");


        // ConfigureAwait (library-safe)
        async Task<Order> GetOrderSafe(int id)
        {
            return await service.GetOrderAsync(id).ConfigureAwait(false);
        }

        await GetOrderSafe(99);
    }
}