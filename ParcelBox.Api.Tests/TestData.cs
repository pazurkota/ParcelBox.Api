using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public static class TestData
{
    public static CreateLockerDto CreateLockerDto(string code = "WAS-002") =>
        new CreateLockerDto
        {
            Code = code,
            Address = "1600 Pennsylvania Avenue NW",
            City = "Washington DC",
            PostalCode = "20500 USA"
        };

    public static Locker LockerEntity(int id = 1, string code = "WAS-001") =>
        new Locker
        {
            Id = id,
            Code = code,
            Address = "1400 Defense Pentagon",
            City = "Washington DC",
            PostalCode = "20301 USA",
            LockerBoxes = new List<LockerBox>()
        };
}