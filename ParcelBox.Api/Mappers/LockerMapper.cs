using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Mappers;

public static class LockerMapper
{
    public static GetLockersDto LockersToGetLockersRequestDto(Locker locker)
    {
        return new GetLockersDto
        {
            Id = locker.Id,
            Code = locker.Code,
            Address = locker.Address,
            City = locker.City,
            PostalCode = locker.PostalCode,
            LockerBoxes = locker.LockerBoxes
                .Select(x => new GetLockerBoxDto()
                {
                    Id = x.Id,
                    LockerSize = x.LockerSize,
                    IsOccupied = x.IsOccupied,
                    LockerId = x.LockerId
                }).ToList()
        };
    }
}