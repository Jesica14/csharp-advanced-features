using System.Runtime.CompilerServices;
using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("UnitTesting")]

namespace EntityFramework.Services;

public class ReservationService
{
    private readonly HotelDbContext _context;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(HotelDbContext context, ILogger<ReservationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
    {
        _logger.LogInformation("Fetching available rooms from {CheckIn} to {CheckOut}", checkIn, checkOut);

        return await _context.Rooms
            .AsNoTracking()
            .Where(r => r.IsAvailable && !_context.Reservations.Any(res =>
                res.RoomId == r.Id &&
                res.CheckIn < checkOut &&
                res.CheckOut > checkIn))
            .OrderBy(r => r.PricePerNight)
            .ToListAsync();
    }

    public async Task<Reservation> CreateReservationAsync(
        int guestId,
        int roomId,
        DateTime checkIn,
        DateTime checkOut)
    {
        if (!IsValidDateRange(checkIn, checkOut))
        {
            throw new ArgumentException("Invalid date range.");
        }

        var room = await _context.Rooms.FindAsync(roomId)
            ?? throw new InvalidOperationException("Room not found.");

        if (!room.IsAvailable)
        {
            _logger.LogWarning("Room {Room} is not available", room.Number);
            throw new InvalidOperationException($"Room {room.Number} is not available.");
        }

        var hasConflict = await _context.Reservations.AnyAsync(res =>
            res.RoomId == roomId &&
            res.CheckIn < checkOut &&
            res.CheckOut > checkIn);

        if (hasConflict)
        {
            throw new InvalidOperationException("Room is already booked for selected dates.");
        }

        var total = CalculateTotalPrice(checkIn, checkOut, room.PricePerNight);

        var reservation = new Reservation
        {
            GuestId = guestId,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            TotalPrice = total
        };

        try
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Reservation #{Id} created for guest {GuestId}", reservation.Id, guestId);
            return reservation;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation for guest {GuestId}", guestId);
            throw;
        }
    }

    public async Task CancelReservationAsync(int reservationId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId)
            ?? throw new InvalidOperationException("Reservation not found.");

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Reservation #{Id} cancelled", reservationId);
    }

    internal decimal CalculateTotalPrice(DateTime checkIn, DateTime checkOut, decimal pricePerNight)
    {
        var nights = (checkOut - checkIn).Days;

        if (nights <= 0)
        {
            throw new ArgumentException("CheckOut must be after CheckIn.");
        }

        return nights * pricePerNight;
    }

    private static bool IsValidDateRange(DateTime checkIn, DateTime checkOut) =>
        checkOut > checkIn && checkIn >= DateTime.Today;
}