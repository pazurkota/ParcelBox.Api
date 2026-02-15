using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Locker> Lockers { get; set; }
    public DbSet<LockerBox> LockerBoxes { get; set; }
    public DbSet<Parcel> Parcels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Locker>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<LockerBox>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Parcel>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
    }
}