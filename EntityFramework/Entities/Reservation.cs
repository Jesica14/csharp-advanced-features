namespace HotelEF.Entities;

// Fluent API in HotelDbContext
public class Reservation
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Notes { get; set; }

    public Guest Guest { get; set; } = null!;
    public Room Room { get; set; } = null!;
}
