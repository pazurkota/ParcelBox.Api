using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
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
    
    public static CreateLockerDto InvalidCreateLockerDto() => new();

    public static Locker LockerEntity() =>
        new()
        {
            Code = "WAS-001",
            Address = "1400 Defense Pentagon",
            City = "Washington DC",
            PostalCode = "20301 USA",
            LockerBoxes = new List<LockerBox>
            {
                new() {LockerSize = Size.Small},
            }
        };

    public static EditLockerDto EditLockerDto() =>
        new()
        {
            Address = "800 Southern Ave SE"
        };

    public static EditLockerDto InvalidEditLockerDto() => new();

    public static CreateLockerBoxesDtos CreateLockerBoxDtos() =>
        new()
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() {LockerSize = "Small"},
                new() {LockerSize = "Medium"},
                new() {LockerSize = "Big"}
            }
        };
    
    public static CreateLockerBoxesDtos CreateInvalidLockerBoxDtos() =>
        new()
        {
            BoxDtos = new List<CreateLockerBoxDto>
            {
                new() {LockerSize = "Smol"},
                new() {LockerSize = "Large"}
            }
        };
}