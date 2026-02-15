using FluentValidation;
using ParcelBox.Api.Dtos.Parcel;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Validation.ParcelsValidation;

public class EditParcelDtoValidator : AbstractValidator<EditParcelDto>
{
    public EditParcelDtoValidator()
    {
        RuleFor(x => x.ParcelStatus)
            .NotEmpty()
            .IsEnumName(typeof(Status), caseSensitive: false)
            .WithMessage("Status given was invalid. Valid statues: (Small, Medium, Big)");
    }
}