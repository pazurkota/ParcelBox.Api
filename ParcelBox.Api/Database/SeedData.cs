using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Database;

public static class SeedData
{
    public static void Seed(IServiceProvider provider)
    {
        var context = provider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        if (!context.Lockers.Any())
        {
            context.Lockers.AddRange(
                new Locker
                {
                    Address = "1400 Defense Pentagon",
                    City = "Washington DC",
                    PostalCode = "20301 USA",
                    Code = "WAS-001",
                    LockerBoxes =
                    [
                        new LockerBox
                        {
                            LockerSize = Size.Small,
                            IsOccupied = false
                        },

                        new LockerBox
                        {
                            LockerSize = Size.Medium,
                            IsOccupied = true
                        },

                        new LockerBox
                        {
                            LockerSize = Size.Big,
                            IsOccupied = false
                        }
                    ]
                });
            
            context.SaveChanges();
        }
    }
}