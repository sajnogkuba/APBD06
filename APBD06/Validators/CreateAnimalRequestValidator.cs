using APBD06.DTOs;
using FluentValidation;

namespace APBD06.Validators;

public class CreateAnimalRequestValidator : AbstractValidator<CreateAnimalRequest>
{
    public CreateAnimalRequestValidator()
    {
        RuleFor(e => e.Name).MaximumLength(50).NotNull();
        RuleFor(e => e.Description).MaximumLength(50);
        RuleFor(e => e.Category).MaximumLength(50).NotNull();
        RuleFor(e => e.Area).MaximumLength(50).NotNull();
    }
}