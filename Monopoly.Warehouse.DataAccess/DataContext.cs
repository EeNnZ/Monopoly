using Microsoft.EntityFrameworkCore;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.DataAccess.EntityConfigurations;

namespace Monopoly.Warehouse.DataAccess;

public class DataContext : DbContext
{
    public DbSet<Pallet> Pallets { get; set; }
    public DbSet<Box> Boxes { get; set; }

    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PalletEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BoxEntityConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        var boxes = ChangeTracker.Entries<Box>();

        foreach (var box in boxes)
        {
            if (box.State is EntityState.Added or EntityState.Modified)
            {
                var entity = box.Entity;
                if (entity.DateCreated is null && entity.ExpirationDate is null)
                {
                    throw new InvalidOperationException($"At least one of {nameof(entity.DateCreated)} or {entity.ExpirationDate} is required.");
                }
            }
        }
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var boxes = ChangeTracker.Entries<Box>();

        foreach (var box in boxes)
        {
            if (box.State is EntityState.Added or EntityState.Modified)
            {
                var entity = box.Entity;
                if (entity.DateCreated is null && entity.ExpirationDate is null)
                {
                    throw new InvalidOperationException($"At least one of {nameof(entity.DateCreated)}  or {entity.ExpirationDate} is required.");
                }
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}