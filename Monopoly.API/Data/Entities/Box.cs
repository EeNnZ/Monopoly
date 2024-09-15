using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Monopoly.API.Data.DTOs;

namespace Monopoly.API.Data.Entities;

/// <summary>
///     Represents a box in a warehouse, inheriting from the WarehouseObject class.
///     Implements ILimitedExpiration to manage expiration-related properties.
/// </summary>
[Index(nameof(Id), IsUnique = true)]
public class Box : WarehouseObject
{
    private DateOnly? _expirationDate;

    /// <summary>
    ///     Default constructor
    /// </summary>
    public Box()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Box class from given <see cref="BoxDto"/>.
    /// </summary>
    /// <param name="dto"></param>
    public Box(BoxDto dto) : this(dto.Weight, dto.DateCreated, dto.ExpirationDate)
    {
        Id       = dto.Id;
        Width    = dto.Width;
        Height   = dto.Height;
        Depth    = dto.Depth;
        PalletId = dto.PalletId;
    }

    /// <summary>
    ///     Initializes a new instance of the Box class with specified weight, date created, and expiration date.
    /// </summary>
    /// <param name="weight">The weight of the box.</param>
    /// <param name="dateCreated">The date the box was created (optional).</param>
    /// <param name="expirationDate">The expiration date of the box (optional).</param>
    /// <exception cref="ValidationException">Thrown if both dateCreated and expirationDate are null.</exception>
    public Box(decimal weight, DateOnly? dateCreated = null, DateOnly? expirationDate = null)
    {
        Weight = weight;

        // Validate that at least one of the dates is provided
        if (dateCreated is null && expirationDate is null)
            throw new ValidationException("Creation or expiration date must be specified");

        // Set the DateCreated if it is provided
        if (dateCreated is not null)
            DateCreated = dateCreated;

        // Set the expiration date if the dateCreated is null
        // and expirationDate is provided
        if (dateCreated is null && expirationDate.HasValue)
            ExpirationDate = expirationDate.Value;
    }

    /// <summary>
    ///     Gets or sets the date when the box was created.
    ///     This property is nullable.
    /// </summary>
    public DateOnly? DateCreated { get; set; }

    /// <summary>
    ///     Gets the weight of the box.
    ///     This property is overridden from WarehouseObject class.
    /// </summary>
    public override decimal Weight { get; }

    /// <summary>
    ///     Gets or sets the ID of the pallet that this box is associated with.
    /// </summary>
    public long? PalletId { get; set; }

    /// <summary>
    ///     Navigation property for the associated Pallet entity.
    /// </summary>
    [ForeignKey(nameof(PalletId))]
    public virtual Pallet? Pallet { get; set; }

    /// <summary>
    ///     Gets the volume of the box calculated using the given dimensions (Width, Height, Depth).
    ///     This property is not mapped to the database.
    /// </summary>
    [NotMapped]
    public override decimal Volume => Width * Height * Depth;

    /// <summary>
    ///     Gets or sets the expiration date of the box.
    ///     If DateCreated is not null, the expiration date is calculated as 100 days after DateCreated.
    /// </summary>
    public DateOnly? ExpirationDate
    {
        get => DateCreated?.AddDays(100) ?? _expirationDate;

        set
        {
            // Set the expiration date only if DateCreated is null
            if (DateCreated is null)
                _expirationDate = value;
        }
    }
}