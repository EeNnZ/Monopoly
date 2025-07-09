using FluentValidation;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.WebHost.Models.Box;

namespace Monopoly.Warehouse.WebHost.Validation;

public class BoxCreateOrUpdateValidator : AbstractValidator<BoxCreateOrUpdate>
{
    public BoxCreateOrUpdateValidator()
    {
        RuleFor(box => box.Weight).NotNull().GreaterThan(0);
        RuleFor(box => box).Must(HasCreationOrExpirationDate);
        // todo other constraints for box
    }

    private bool HasCreationOrExpirationDate(BoxCreateOrUpdate arg)
    {
        return arg.DateCreated is not null || arg.ExpirationDate is not null;
    }
}