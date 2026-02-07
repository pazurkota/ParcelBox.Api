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
        for (int i = 0; i < 3; i++) code[i] = letters[random.Next(0, letters.Length - 1)];

        // generate random numbers
        for (int i = 3; i < 8; i++) code[i] = numbers[random.Next(0, numbers.Length - 1)];

        return new string(code);
    }

    // @TODO Fix LINQ expressions
    public static async Task<(int initalLockerBoxId, int targetLockerBoxId)> SetLockerBoxes
        (AppDbContext context, int initId, int targetId)
    {
        var initLockerBoxId = await context.LockerBoxes
            .Where(x => 
                x.IsOccupied == false &&
                x.LockerId == initId)
            .FirstOrDefaultAsync();
        
        var targetLockerBoxId = await context.LockerBoxes
            .Where(x =>
                x.IsOccupied == false &&
                x.LockerId == targetId)
            .FirstOrDefaultAsync();

        if (initLockerBoxId is null) 
            throw new NoNullAllowedException("Initial locker box is null");
        
        if (targetLockerBoxId is null) 
            throw new NoNullAllowedException("Target locker box is null");
        
        return (initLockerBoxId.Id, targetLockerBoxId.Id);
    }
}