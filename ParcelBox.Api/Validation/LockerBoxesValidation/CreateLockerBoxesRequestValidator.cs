using FluentValidation;
using ParcelBox.Api.Dtos.LockerBox;

namespace ParcelBox.Api.Validation.LockerBoxesValidation;

public class CreateLockerBoxesRequestValidator : AbstractValidator<CreateLockerBoxesDtos>
{
    public CreateLockerBoxesRequestValidator()
    {
        RuleFor(x => x.BoxDtos)
            .NotEmpty()
            .OverridePropertyName("lockerBoxes");
        
        RuleForEach(x => x.BoxDtos)
            .SetValidator(new CreateLockerBoxValidator());
    }
}