using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

namespace Monopoly.Warehouse.DataAccess.EntityConfigurations;

public class BoxEntityConfiguration : IEntityTypeConfiguration<Box>
{
    public void Configure(EntityTypeBuilder<Box> b)
    {
        b.ToTable("Boxes");
        
        b.HasKey(x => x.Id);
        
        b.Property(x => x.Width).IsRequired();
        b.Property(x => x.DateCreated);
        b.Property(x => x.ExpirationDate);
        b.Property(x => x.Width);
        b.Property(x => x.Height);
        b.Property(x => x.Depth);
        
        b.HasOne<Pallet>(box => box.Pallet)
         .WithMany(pallet => pallet.Boxes)
         .HasForeignKey(x => x.PalletId);
    }
}