using FluentValidation;
using ParcelBox.Api.Dtos.Parcel;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Validation.ParcelsValidation;

public class CreateParcelDtoValidator : AbstractValidator<CreateParcelDto>
{
    public CreateParcelDtoValidator()
    {
        RuleFor(x => x.ParcelSize)
            .NotEmpty()
            .IsEnumName(typeof(Size), caseSensitive: false)
            .WithMessage("Size given was invalid. Valid sizes: (Small, Medium, Big)");

        RuleFor(x => x.InitialLockerId).NotEmpty();
        RuleFor(x => x.TargetLockerId).NotEmpty();
    }
}