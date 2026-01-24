using ParcelBox.Api.Model;

namespace ParcelBox.Api.Database;

public static class SeedData
{
    public static void Seed(IServiceProvider provider)
    {
        var context = provider.GetRequiredService<AppDbContext>();

        if (!context.Lockers.Any())
        {
            context.Lockers.AddRange(
                new Locker
                {
                    Id = 1,
                    Address = "1400 Defense Pentagon",
                    City = "Washington DC",
                    PostalCode = "20301 USA",
                    Code = "WAS-001",
                    LockerBoxes =
                    [
                        new LockerBox
                        {
                            Id = 1,
                            LockerSize = Size.Small,
                            IsOccupied = false,
                            LockerId = 1
                        },

                        new LockerBox
                        {
                            Id = 2,
                            LockerSize = Size.Medium,
                            IsOccupied = true,
                            LockerId = 1
                        },

                        new LockerBox
                        {
                            Id = 3,
                            LockerSize = Size.Big,
                            IsOccupied = false,
                            LockerId = 1
                        }
                    ]
                });
            
            context.SaveChanges();
        }
    }
}