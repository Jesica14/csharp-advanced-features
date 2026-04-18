using System.Diagnostics;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        // Data
        var orders = Enumerable.Range(1, 10_000)
            .Select(i => new { Id = i, Customer = $"Customer_{i}", Amount = i * 10m, IsPaid = i % 2 == 0 })
            .ToList();

        var sw = new Stopwatch();

        // String concatenation
        sw.Start();

        string report = string.Empty;
        foreach (var o in orders)
        {
            report += $"Order #{o.Id} | {o.Customer} | {o.Amount:C} | Paid={o.IsPaid}\n";
            Console.WriteLine(report);
        }

        sw.Stop();

        Console.WriteLine($"Concat: {sw.ElapsedMilliseconds} ms");


        // StringBuilder
        sw.Restart();

        var sb = new StringBuilder();
        foreach (var o in orders)
        {
            sb.AppendLine($"Order #{o.Id} | {o.Customer} | {o.Amount:C} | Paid={o.IsPaid}");
        }

        var result = sb.ToString();

        sw.Stop();
        Console.WriteLine($"StringBuilder: {sw.ElapsedMilliseconds} ms");


        // StringBuilder with capacity
        sw.Restart();

        var sb2 = new StringBuilder(orders.Count * 60);
        foreach (var o in orders)
        {
            sb2.AppendLine($"Order #{o.Id} | {o.Customer} | {o.Amount:C} | Paid={o.IsPaid}");
        }

        sb2.ToString();

        sw.Stop();
        Console.WriteLine($"StringBuilder (capacity): {sw.ElapsedMilliseconds} ms");
    }
}