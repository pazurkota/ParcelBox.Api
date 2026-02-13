using ParcelBox.Api.Dtos.Parcel;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Mappers;

public static class ParcelMapper
{
    public static GetParcelDto ParcelToGetParcelDto(Parcel parcel)
    {
        return new GetParcelDto
        {
            Id = parcel.Id,
            
            PickupCode = parcel.PickupCode,
            ParcelSize = parcel.ParcelSize.ToString(),
            
            InitialLockerId = parcel.InitialLockerId,
            TargetLockerId = parcel.TargetLockerId,
            
            InitialLockerBoxId = parcel.InitialLockerBoxId,
            TargetLockerBoxId = parcel.TargetLockerBoxId
        };
    }
}