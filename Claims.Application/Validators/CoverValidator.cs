using Claims.Models;
using FluentValidation;

namespace Claims.Application.Validators;

public class CoverValidator : AbstractValidator<Cover>
{
    public CoverValidator()
    {
        RuleFor(cover => cover.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Start date cannot be in the past.");

        RuleFor(cover => cover.EndDate)
            .GreaterThan(cover => cover.StartDate)
            .Must((cover, endDate) => (endDate - cover.StartDate).Days <= 365).WithMessage("Total insurance period cannot exceed 1 year.");

        RuleFor(cover => cover.Type)
            .IsInEnum();

        RuleFor(cover => cover.Premium)
            .GreaterThanOrEqualTo(0);
    }
}
