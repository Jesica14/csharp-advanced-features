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

        Console.WriteLine("── Available rooms");
        var available = await reservationService.GetAvailableRoomsAsync(
            DateTime.Today.AddDays(6),
            DateTime.Today.AddDays(9));

        foreach (var r in available)
        {
            Console.WriteLine($"  Room {r.Number} | {r.Type} | {r.PricePerNight:C}/night");
        }

        Console.WriteLine("\n── Create reservation");
        var roomToBook = await db.Rooms.FirstAsync(r => r.Number == "104");
        var guest = await db.Guests.FirstAsync(g => g.Email == "luis@hotel.com");

        try
        {
            var reservation = await reservationService.CreateReservationAsync(
                guest.Id, roomToBook.Id,
                DateTime.Today.AddDays(7),
                DateTime.Today.AddDays(9));

            Console.WriteLine($"  Created reservation #{reservation.Id} | Total: {reservation.TotalPrice:C}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  {ex.Message}");
        }

        Console.WriteLine("\n── Tracking: update room availability");
        var room = await db.Rooms.FirstAsync(r => r.Number == "101");
        room.IsAvailable = false;
        await db.SaveChangesAsync();
        Console.WriteLine($"  Room {room.Number} marked as unavailable");
    }
}