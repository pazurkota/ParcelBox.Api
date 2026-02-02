using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Database;

public static class SeedData
{
    public static void MigrateAndSeed(IServiceProvider provider)
    {
        var context = provider.GetRequiredService<AppDbContext>();
        
        context.Database.EnsureDeleted(); // temp only
        context.Database.Migrate();
        
        var locker = context.Lockers.FirstOrDefault(x => x.Code == "WAS-001");

        if (locker == null)
        {
            locker = new Locker
            {
                Address = "1400 Defense Pentagon",
                City = "Washington DC",
                PostalCode = "20301 USA",
                Code = "WAS-001",
            };
            
            context.Lockers.Add(locker);
            context.SaveChanges();
        }
        
        if (!context.LockerBoxes.Any(x => x.LockerId == locker.Id))
        {
            List<LockerBox> lockerBoxes = new()
            {
                new()
                {
                    LockerSize = Size.Small,
                    IsOccupied = false,
                    LockerId = locker.Id
                },
                
                new()
                {
                    LockerSize = Size.Medium,
                    IsOccupied = true,
                    LockerId = locker.Id
                },
                
                new()
                {
                    LockerSize = Size.Big,
                    IsOccupied = false,
                    LockerId = locker.Id
                }
            };

            context.LockerBoxes.AddRange(lockerBoxes);
            context.SaveChanges();
        }
    }
}