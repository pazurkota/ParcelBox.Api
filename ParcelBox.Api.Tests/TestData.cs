using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public static class TestData
{
    public static CreateLockerDto CreateLockerDto() =>
        new()
        {
            Code = "WAS-002",
            Address = "1600 Pennsylvania Avenue NW",
            City = "Washington DC",
            PostalCode = "20500 USA"
        };

    public static Locker LockerEntity() =>
        new()
        {
            Id = 1,
            Code = "WAS-001",
            Address = "1400 Defense Pentagon",
            City = "Washington DC",
            PostalCode = "20301 USA",
            LockerBoxes = new List<LockerBox>()
        };

    public static CreateLockerBoxDto[] CreateLockerBoxDtos() =>
    [
        new() {LockerSize = "Small"},
        new() {LockerSize = "Medium"},
        new() {LockerSize = "Big"}
    ];
}