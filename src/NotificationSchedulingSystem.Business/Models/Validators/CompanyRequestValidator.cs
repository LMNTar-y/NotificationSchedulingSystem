using FluentValidation;
using NotificationSchedulingSystem.Infrastructure.Enums;

namespace NotificationSchedulingSystem.Business.Models.Validators;

public class CompanyRequestValidator : AbstractValidator<CompanyRequest>
{
    public CompanyRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 20);
        RuleFor(x => x.Number).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Market).IsEnumName(typeof(Market), false);
        RuleFor(x => x.CompanyType).IsEnumName(typeof(CompanyType), false);
    }
}