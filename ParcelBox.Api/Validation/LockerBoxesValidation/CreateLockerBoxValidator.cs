using FluentValidation;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Validation.LockerBoxesValidation;

public class CreateLockerBoxValidator : AbstractValidator<CreateLockerBoxDto>
{
    public CreateLockerBoxValidator()
    {
        RuleFor(x => x.LockerSize)
            .NotEmpty().
            WithMessage("The locker size is required")
            .IsInEnum().
            WithMessage($"Locker size must be one of: {string.Join(", ", Enum.GetNames<Size>())}");
    }
}