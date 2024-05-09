using Claims.Models;
using FluentValidation;

namespace Claims.Application.Validators;

public class ClaimValidator : AbstractValidator<Claim>
{
    public ClaimValidator()
    {
        RuleFor(claim => claim.CoverId)
            .NotEmpty().WithMessage("Cover ID is required.");

        RuleFor(claim => claim.Name)
            .NotEmpty();

        RuleFor(claim => claim.Type)
            .IsInEnum();

        RuleFor(claim => claim.DamageCost)
            .InclusiveBetween(0, 100M).WithMessage("Damage cost must be between 0 and 100.000.");
    }
}
