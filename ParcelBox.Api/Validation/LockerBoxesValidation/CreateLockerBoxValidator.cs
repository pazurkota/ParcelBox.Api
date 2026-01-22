using FluentValidation;
using ParcelBox.Api.Dtos.Locker;
using ParcelBox.Api.Dtos.LockerBox;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Validation.LockerBoxesValidation;

public class CreateLockerBoxValidator : AbstractValidator<CreateLockerBoxDto>
{
    public CreateLockerBoxValidator()
    {
        RuleFor(x => x.LockerSize)
            .NotEmpty()
            .IsEnumName(typeof(Size), caseSensitive: false)
            .WithMessage("Size given was invalid. Valid sizes: (Small, Medium, Big)");
    }
}