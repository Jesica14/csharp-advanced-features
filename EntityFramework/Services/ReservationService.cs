using HotelEF.Data;
using HotelEF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelEF.Services;

public class ReservationService
{
    private readonly HotelDbContext _context;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(HotelDbContext context, ILogger<ReservationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task RunDemoAsync()
    {
        var checkIn = DateTime.Today.AddDays(6);
        var checkOut = DateTime.Today.AddDays(9);

        // Read-only query (no tracking)
        _logger.LogInformation(
            "Fetching available rooms from {CheckIn} to {CheckOut}",
            checkIn,
            checkOut);

        var rooms = await _context.Rooms
            .AsNoTracking()
            .Where(r => r.IsAvailable &&
                        !_context.Reservations.Any(res =>
                            res.RoomId == r.Id &&
                            res.CheckIn < checkOut &&
                            res.CheckOut > checkIn))
            .OrderBy(r => r.PricePerNight)
            .ToListAsync();

        foreach (var room in rooms)
        {
            Console.WriteLine($"  Room {room.Number} | {room.Type} | {room.PricePerNight:C}/night");
        }

        // Create reservation (tracking required)
        var roomToBook = await _context.Rooms.FirstAsync(r => r.Number == "104");
        var guest = await _context.Guests.FirstAsync(g => g.Email == "luis@hotel.com");

        if (!roomToBook.IsAvailable)
        {
            _logger.LogWarning("Room {Room} is not available", roomToBook.Number);
            return;
        }

        var reservation = new Reservation
        {
            RoomId = roomToBook.Id,
            GuestId = guest.Id,
            CheckIn = DateTime.Today.AddDays(7),
            CheckOut = DateTime.Today.AddDays(9),
            Notes = "Created from demo",
            TotalPrice = 1700
        };

        try
        {
            _logger.LogInformation(
                "Creating reservation for {Guest} in room {Room}",
                guest.FullName,
                roomToBook.Number);

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            Console.WriteLine(
                $"  Created reservation #{reservation.Id} | Total: {reservation.TotalPrice:C}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation");
            throw;
        }
    }
}