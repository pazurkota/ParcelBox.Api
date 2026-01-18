using FluentValidation;
using ParcelBox.Api.Dtos.Locker;

namespace ParcelBox.Api.Validation.LockersValidation;

public class EditLockerDtoValidator : AbstractValidator<EditLockerDto>
{
    public EditLockerDtoValidator()
    {
        RuleFor(x => x.Address).NotEmpty().WithMessage("Edited address cannot be null.");
    }
}