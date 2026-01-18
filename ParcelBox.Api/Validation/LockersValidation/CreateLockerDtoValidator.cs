using FluentValidation;
using ParcelBox.Api.Dtos.Locker;

namespace ParcelBox.Api.Validation.LockersValidation;

public class CreateLockerDtoValidator : AbstractValidator<CreateLockerDto>
{
    public CreateLockerDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Locker code must be provided.")
            .Matches(@"^[A-Z]{3}-\d{3}$")
            .WithMessage("Locker code must be in format: XXX-000 (e.g., WAS-001).");
        
        RuleFor(x => x.Address).NotEmpty().WithMessage("Locker address must be provided.");
        RuleFor(x => x.City).NotEmpty().WithMessage("Locker city must be provided.");
        RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Locker postal code must be provided");
    }
}