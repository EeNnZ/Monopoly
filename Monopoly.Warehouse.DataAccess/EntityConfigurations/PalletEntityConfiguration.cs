using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

namespace Monopoly.Warehouse.DataAccess.EntityConfigurations;

public class PalletEntityConfiguration : IEntityTypeConfiguration<Pallet>
{
    public void Configure(EntityTypeBuilder<Pallet> b)
    {
        b.ToTable("Pallets");
        
        b.HasKey(x => x.Id);
        
        b.Property(x => x.Width);
        b.Property(x => x.Height);
        b.Property(x => x.Depth);
    }
}