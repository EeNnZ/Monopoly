using FluentValidation;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.WebHost.Models.Pallet;

namespace Monopoly.Warehouse.WebHost.Validation;

public class PalletCreateOrUpdateValidator : AbstractValidator<PalletCreateOrUpdate>
{
    public PalletCreateOrUpdateValidator()
    {
        RuleFor(x => x.Width).NotNull().GreaterThan(0);
        RuleFor(x => x.Height).NotNull().GreaterThan(0);
        RuleFor(x => x.Depth).NotNull().GreaterThan(0);
    }
}