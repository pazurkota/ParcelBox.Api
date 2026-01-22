using System.Collections.Generic;

namespace ParcelBox.Api.Dtos.LockerBox;

public class CreateLockerBoxesDtos
{
    public List<CreateLockerBoxDto> BoxDtos { get; set; } = [];
}