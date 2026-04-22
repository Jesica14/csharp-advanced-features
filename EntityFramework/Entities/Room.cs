using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities;

public class Room
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string Number { get; set; } = string.Empty;

    [Required]
    public RoomType Type { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerNight { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }

    public bool IsAvailable { get; set; } = true;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}