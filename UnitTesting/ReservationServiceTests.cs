using System.Reflection;
using EntityFramework.Data;
using EntityFramework.Entities;
using EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace UnitTesting;

public class ReservationServiceTests
{
    private static HotelDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new HotelDbContext(options);
    }

    // InternalsVisibleTo allows testing internal methods without reflection
    [Test]
    public void CalculateTotalPrice_ReturnsCorrectAmount()
    {
        // Arrange
        var service = new ReservationService(null!, NullLogger<ReservationService>.Instance);

        // Act
        var total = service.CalculateTotalPrice(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 1, 3),
            100);

        // Assert
        Assert.That(total, Is.EqualTo(200));
    }

    [Test]
    public void CalculateTotalPrice_InvalidDates_Throws()
    {
        // Arrange
        var service = new ReservationService(null!, NullLogger<ReservationService>.Instance);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            service.CalculateTotalPrice(DateTime.Today, DateTime.Today, 100));
    }

    // Reflection — private method
    [Test]
    public void IsValidDateRange_ValidDates_ReturnsTrue()
    {
        // Arrange
        var method = typeof(ReservationService)
            .GetMethod("IsValidDateRange", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new MissingMethodException("IsValidDateRange not found.");

        // Act
        var result = (bool)method.Invoke(null, [DateTime.Today.AddDays(1), DateTime.Today.AddDays(3)])!;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidDateRange_CheckInInThePast_ReturnsFalse()
    {
        // Arrange
        var method = typeof(ReservationService)
            .GetMethod("IsValidDateRange", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new MissingMethodException("IsValidDateRange not found.");

        // Act
        var result = (bool)method.Invoke(null, [DateTime.Today.AddDays(-1), DateTime.Today.AddDays(3)])!;

        // Assert
        Assert.That(result, Is.False);
    }

    // Public method tests
    [Test]
    public async Task CreateReservationAsync_CreatesReservation()
    {
        // Arrange
        var context = CreateContext();
        var guest = new Guest { FullName = "Test", Email = "test@test.com" };
        var room = new Room  { Number = "101", Type = RoomType.Standard, PricePerNight = 100 };

        context.AddRange(guest, room);
        await context.SaveChangesAsync();

        var service = new ReservationService(context, NullLogger<ReservationService>.Instance);

        // Act
        var reservation = await service.CreateReservationAsync(
            guest.Id, room.Id,
            DateTime.Today,
            DateTime.Today.AddDays(2));

        // Assert
        Assert.That(reservation, Is.Not.Null);
        Assert.That(reservation.TotalPrice, Is.EqualTo(200));
    }

    [Test]
    public void CreateReservationAsync_WhenRoomNotAvailable_Throws()
    {
        // Arrange
        var context = CreateContext();
        var guest = new Guest { FullName = "Test", Email = "test@test.com" };
        var room = new Room { Number = "101", Type = RoomType.Standard, PricePerNight = 100, IsAvailable = false };

        context.AddRange(guest, room);
        context.SaveChanges();

        var service = new ReservationService(context, NullLogger<ReservationService>.Instance);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateReservationAsync(
                guest.Id, room.Id,
                DateTime.Today,
                DateTime.Today.AddDays(2)));
    }

    // Fixed: tests GetAvailableRoomsAsync which is where overlap is validated
    [Test]
    public async Task GetAvailableRoomsAsync_ExcludesOverlappingReservations()
    {
        // Arrange
        var context = CreateContext();
        var guest   = new Guest { FullName = "Test", Email = "test@test.com" };
        var room    = new Room  { Number = "101", Type = RoomType.Standard, PricePerNight = 100 };

        context.AddRange(guest, room);
        context.SaveChanges();

        context.Reservations.Add(new Reservation
        {
            GuestId = guest.Id,
            RoomId = room.Id,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(3),
            TotalPrice = 300
        });
        context.SaveChanges();

        var service = new ReservationService(context, NullLogger<ReservationService>.Instance);

        // Act
        var available = await service.GetAvailableRoomsAsync(
            DateTime.Today.AddDays(1),
            DateTime.Today.AddDays(4));

        // Assert
        Assert.That(available, Is.Empty);
    }
}
