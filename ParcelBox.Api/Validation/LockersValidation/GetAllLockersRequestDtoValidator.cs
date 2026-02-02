using FluentValidation;
using ParcelBox.Api.Dtos.Locker;

namespace ParcelBox.Api.Validation.LockersValidation;

public class GetAllLockersRequestDtoValidator : AbstractValidator<GetAllLockersRequestDto>
{
    public GetAllLockersRequestDtoValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.RecordsPerPage)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);
    }
}