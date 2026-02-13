using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Database;

public static class MigrateAndSeedData
{
    public static void MigrateAndSeed(IServiceProvider provider)
    {
        var context = provider.GetRequiredService<AppDbContext>();
        
        context.Database.Migrate();

        List<Locker> lockers = new();
        
        if (!context.Lockers.Any())
        {
            lockers = new()
            {
                new()
                {
                    Address = "1400 Defense Pentagon",
                    City = "Washington DC",
                    PostalCode = "20301 USA",
                    Code = "WAS-001",
                },
                new()
                {
                    Address = "608 Rhode Island Ave",
                    City = "Washington DC",
                    PostalCode = "20002 USA",
                    Code = "WAS-002",
                },
            };
            
            context.Lockers.AddRange(lockers);
            context.SaveChanges();
        }
        
        if (lockers.Count >= 2 && 
            !context.LockerBoxes.Any(x => x.LockerId == lockers[0].Id) &&
            !context.LockerBoxes.Any(x => x.LockerId == lockers[1].Id))
        {
            List<LockerBox> lockerBoxes = new()
            {
                new()
                {
                    LockerSize = Size.Small,
                    IsOccupied = false,
                    LockerId = lockers[0].Id
                },
                
                new()
                {
                    LockerSize = Size.Medium,
                    IsOccupied = false,
                    LockerId = lockers[0].Id
                },
                
                new()
                {
                    LockerSize = Size.Big,
                    IsOccupied = false,
                    LockerId = lockers[0].Id
                },
                
                new()
                {
                    LockerSize = Size.Small,
                    IsOccupied = false,
                    LockerId = lockers[1].Id
                },
                
                new()
                {
                    LockerSize = Size.Medium,
                    IsOccupied = false,
                    LockerId = lockers[1].Id
                },
                
                new()
                {
                    LockerSize = Size.Big,
                    IsOccupied = false,
                    LockerId = lockers[1].Id
                }
            };

            context.LockerBoxes.AddRange(lockerBoxes);
            context.SaveChanges();
        }
        
        if (!context.Parcels.Any())
        {
            Parcel parcel = new()
            {
                PickupCode = "ABC00000",
                ParcelSize = Size.Small,
                InitialLockerId = 1,
                TargetLockerId = 2,
                InitialLockerBoxId = 3,
                TargetLockerBoxId = 5
            };

            context.Parcels.Add(parcel);
            context.SaveChanges();
        }
    }
}