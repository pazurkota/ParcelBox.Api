using FluentValidation;
using ParcelBox.Api.Dtos.LockerBox;

namespace ParcelBox.Api.Validation.LockerBoxesValidation;

public class EditLockerBoxStatusDtoValidator : AbstractValidator<EditLockerBoxStatusRequestDto>
{
    public EditLockerBoxStatusDtoValidator()
    {
        RuleFor(x => x.BoxId)
            .NotEmpty()
            .GreaterThan(0);
    }
}