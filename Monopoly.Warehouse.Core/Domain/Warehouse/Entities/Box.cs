using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

/// <summary>
///     Represents a box in a warehouse, inheriting from the WarehouseObject class.
///     Implements ILimitedExpiration to manage expiration-related properties.
/// </summary>
public class Box : WarehouseObject
{
    /// <summary>
    ///     Gets or sets the date when the box was created.
    ///     This property is nullable.
    /// </summary>
    public DateOnly? DateCreated { get; set; }

    /// <summary>
    ///     Gets the weight of the box.
    ///     This property is overridden from WarehouseObject class.
    /// </summary>
    public override decimal Weight { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the pallet that this box is associated with.
    /// </summary>
    public Guid? PalletId { get; set; }

    /// <summary>
    ///     Navigation property for the associated Pallet entity.
    /// </summary>
    public virtual Pallet? Pallet { get; set; }

    /// <summary>
    ///     Gets the volume of the box calculated using the given dimensions (Width, Height, Depth).
    ///     This property is not mapped to the database.
    /// </summary>
    [NotMapped]
    public override decimal Volume => Width * Height * Depth;

    /// <summary>
    ///     Gets or sets the expiration date of the box.
    /// </summary>
    public DateOnly? ExpirationDate { get; set; }
}