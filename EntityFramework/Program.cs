using HotelEF.Data;
using HotelEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json");

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddDbContext<HotelDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("HotelDb")));

        builder.Services.AddScoped<ReservationService>();

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var db = services.GetRequiredService<HotelDbContext>();
        var reservationService = services.GetRequiredService<ReservationService>();

        Console.WriteLine("── Applying migrations...");
        db.Database.Migrate();

        SeedData.Initialize(db);

        Console.WriteLine("Done\n");

        Console.WriteLine("── Demo: queries and reservation");
        await reservationService.RunDemoAsync();

        Console.WriteLine("\n── Tracking: update room availability");

        var room = await db.Rooms.FirstAsync(r => r.Number == "101");
        room.IsAvailable = false;
        await db.SaveChangesAsync();

        Console.WriteLine($"  Room {room.Number} marked as unavailable");

        Console.WriteLine("\n── NoTracking: read-only query");

        var rooms = await db.Rooms
            .AsNoTracking()
            .Where(r => r.IsAvailable)
            .ToListAsync();

        foreach (var r in rooms.Take(3))
        {
            Console.WriteLine($"  Room {r.Number} | {r.Type}");
        }
    }
}