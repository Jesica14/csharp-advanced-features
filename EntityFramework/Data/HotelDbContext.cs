using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Data;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.CheckIn)
                  .HasColumnType("date")
                  .IsRequired();

            entity.Property(r => r.CheckOut)
                  .HasColumnType("date")
                  .IsRequired();

            entity.Property(r => r.TotalPrice)
                  .HasColumnType("decimal(10,2)")
                  .IsRequired();

            entity.Property(r => r.Notes)
                  .HasMaxLength(500);

            entity.HasOne(r => r.Guest)
                  .WithMany(g => g.Reservations)
                  .HasForeignKey(r => r.GuestId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Room)
                  .WithMany(rm => rm.Reservations)
                  .HasForeignKey(r => r.RoomId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
