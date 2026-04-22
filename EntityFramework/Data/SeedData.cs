using HotelEF.Data;
using HotelEF.Entities;

namespace HotelEF.Data;

public static class SeedData
{
    public static void Initialize(HotelDbContext context)
    {
        if (context.Guests.Any()) 
        {
            return;
        }

        var guests = new[]
        {
            new Guest { FullName = "Jesica Villalpando", Email = "jesica@hotel.com", Phone = "5512345678" },
            new Guest { FullName = "Ana Torres", Email = "ana@hotel.com", Phone = "5587654321" },
            new Guest { FullName = "Luis Mendoza", Email = "luis@hotel.com"},
        };

        var rooms = new[]
        {
            new Room { Number = "101", Type = RoomType.Standard, PricePerNight = 800m  },
            new Room { Number = "202", Type = RoomType.Deluxe, PricePerNight = 1500m },
            new Room { Number = "303", Type = RoomType.Suite, PricePerNight = 3000m },
            new Room { Number = "104", Type = RoomType.Standard, PricePerNight = 850m },
        };

        context.Guests.AddRange(guests);
        context.Rooms.AddRange(rooms);
        context.SaveChanges();

        var g1 = context.Guests.First(g => g.Email == "jesica@hotel.com");
        var g2 = context.Guests.First(g => g.Email == "ana@hotel.com");
        var r1 = context.Rooms.First(r => r.Number == "101");
        var r2 = context.Rooms.First(r => r.Number == "202");
        var r3 = context.Rooms.First(r => r.Number == "303");

        context.Reservations.AddRange(
            new Reservation { GuestId = g1.Id, RoomId = r1.Id, CheckIn = DateTime.Today.AddDays(2),  CheckOut = DateTime.Today.AddDays(5),  TotalPrice = 3 * r1.PricePerNight },
            new Reservation { GuestId = g1.Id, RoomId = r2.Id, CheckIn = DateTime.Today.AddDays(10), CheckOut = DateTime.Today.AddDays(12), TotalPrice = 2 * r2.PricePerNight },
            new Reservation { GuestId = g2.Id, RoomId = r3.Id, CheckIn = DateTime.Today.AddDays(1),  CheckOut = DateTime.Today.AddDays(3),  TotalPrice = 2 * r3.PricePerNight }
        );

        context.SaveChanges();

        Console.WriteLine("Seed data inserted.\n");
    }
}
