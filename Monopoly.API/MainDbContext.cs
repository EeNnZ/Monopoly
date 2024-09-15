using Microsoft.EntityFrameworkCore;
using Monopoly.API.Data.Entities;

namespace Monopoly.API;

public class MainDbContext : DbContext
{
    public MainDbContext()
    {
    }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }

    public DbSet<Box>?    Boxes   { get; set; }
    public DbSet<Pallet>? Pallets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Box>()
               .Property(e => e.Rowguid)
               .HasDefaultValueSql("(newid())")
               .HasColumnName("rowguid");
        builder.Entity<Box>()
               .Property(e => e.Depth)
               .HasPrecision(30, 2);
        builder.Entity<Box>()
               .Property(e => e.Width)
               .HasPrecision(30, 2);
        builder.Entity<Box>()
               .Property(e => e.Height)
               .HasPrecision(30, 2);
        builder.Entity<Box>()
               .Property(e => e.Weight)
               .HasPrecision(30, 2);

        builder.Entity<Pallet>()
               .Property(e => e.Rowguid)
               .HasDefaultValueSql("(newid())")
               .HasColumnName("rowguid");
        builder.Entity<Pallet>()
               .Property(e => e.Depth)
               .HasPrecision(30, 2);
        builder.Entity<Pallet>()
               .Property(e => e.Width)
               .HasPrecision(30, 2);
        builder.Entity<Pallet>()
               .Property(e => e.Height)
               .HasPrecision(30, 2);
        builder.Entity<Pallet>()
               .Property(e => e.Weight)
               .HasPrecision(30, 2);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        var boxEntries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is Box
                                    && (e.State    == EntityState.Added
                                        || e.State == EntityState.Modified));

        var palletEntries = ChangeTracker
                           .Entries()
                           .Where(e => e.Entity is Pallet
                                       && (e.State    == EntityState.Added
                                           || e.State == EntityState.Modified));

        return await base.SaveChangesAsync(token);
    }
}