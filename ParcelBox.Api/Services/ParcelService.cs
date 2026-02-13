using System.Data;
using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Database;

namespace ParcelBox.Api.Services;

public static class ParcelService
{
    public static string GenerateParcelCode()
    {
        var random = new Random();
        var letters = "ABCDEFGHIJKLMNOPRSTUWV".ToCharArray();
        var numbers = "0123456789".ToCharArray();
        var code = new char[8]; // parcel code is generated as "XXX00000"
        
        // generate random letters
        for (int i = 0; i < 3; i++) code[i] = letters[random.Next(0, letters.Length)];

        // generate random numbers
        for (int i = 3; i < 8; i++) code[i] = numbers[random.Next(0, numbers.Length)];

        return new string(code);
    }
    
    public static async Task<(int? initialLockerBoxId, int? targetLockerBoxId)> SetLockerBoxesAsync
        (AppDbContext context, int initId, int targetId)
    {
        var initLockerBox = await context.LockerBoxes
            .Where(x => 
                !x.IsOccupied &&
                x.LockerId == initId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

        var targetQuery = context.LockerBoxes
            .Where(x => 
                !x.IsOccupied && 
                x.LockerId == targetId);

        // ensure that init and target locker boxes are different
        // especially if initId == targetId
        targetQuery = targetQuery.Where(x => initLockerBox != null && x.Id != initLockerBox.Id);

        var targetLockerBox = await targetQuery
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();
        
        return (initLockerBox?.Id, targetLockerBox?.Id);
    }

    // put before SaveChangesAsync()
    public static async Task ChangeLockerBoxStatusAsync(AppDbContext context, int id, bool status)
    {
        var lockerBox = await context.LockerBoxes
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (lockerBox is null) throw new NoNullAllowedException("Locker box not found");

        lockerBox.IsOccupied = status;
        
        context.Entry(lockerBox).State = EntityState.Modified;
    }
}