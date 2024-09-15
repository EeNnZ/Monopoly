using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Monopoly.API.Data.DTOs;

namespace Monopoly.API.Data.Entities;

/// <summary>
///     Represents a pallet in a warehouse, which is a special type of
///     WarehouseObject that can contain boxes and has a limited expiration date.
/// </summary>
[Index(nameof(Id), IsUnique = true)]
public class Pallet : WarehouseObject
{
    /// <summary>
    ///     Constant representing the pure weight of the pallet itself without any boxes.
    /// </summary>
    [NotMapped] [JsonIgnore] private const decimal PURE_WEIGHT = 30.0M;

    /// <summary>
    ///     Default constructor
    /// </summary>
    public Pallet()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pallet"/> class from given <see cref="PalletDto"/>.
    /// </summary>
    /// <param name="dto"></param>
    public Pallet(PalletDto dto)
    {
        Id     = dto.Id;
        Width  = dto.Width;
        Height = dto.Height;
        Depth  = dto.Depth;
    }

    /// <summary>
    ///     Collection of boxes that are stored on this pallet.
    ///     Nullable to allow for pallets that may initially not contain any boxes.
    /// </summary>
    public virtual ICollection<Box>? Boxes { get; set; }

    /// <summary>
    ///     Gets the total weight of the pallet, which consists
    ///     of the pure weight of the pallet plus the weight of all boxes on it.
    ///     This property is not stored in the database.
    /// </summary>
    [NotMapped]
    public override decimal Weight
    {
        get
        {
            decimal weight = PURE_WEIGHT;

            // Add the weight of each box if there are any.
            if (Boxes != null && Boxes.Any())
                weight += Boxes.Sum(box => box.Weight);

            return weight;
        }
    }

    /// <summary>
    ///     Gets the total volume of the pallet, which includes the volume of the boxes
    ///     plus the physical dimensions of the pallet itself.
    ///     This property is not stored in the database.
    /// </summary>
    [NotMapped]
    public override decimal Volume
    {
        get
        {
            var boxesVolume = 0.0M;

            // Sum the volume of all boxes if there are any.
            if (Boxes != null && Boxes.Any()) boxesVolume += Boxes.Sum(box => box.Volume);

            // Calculate the total volume including the physical pallet dimensions.
            decimal palletVolume = boxesVolume + Width * Height * Depth;

            return palletVolume;
        }
    }

    /// <summary>
    ///     Gets the earliest expiration date among all boxes on the pallet.
    ///     Returns null if there are no boxes present.
    /// </summary>
    public DateOnly? ExpirationDate
    {
        get
        {
            if (Boxes != null && Boxes.Any())
                return Boxes.Min(x => x.ExpirationDate);

            return null;
        }
    }

    /// <summary>
    ///     Determines whether a given box can fit onto the pallet.
    ///     The box can fit if it fits in either orientation (normal or flipped).
    /// </summary>
    /// <param name="box">The box to check for fitting ability.</param>
    /// <returns>true if the box fits on the pallet; otherwise, false.</returns>
    public bool BoxFits(Box box)
    {
        bool fitsNormal  = Width > box.Width && Depth > box.Depth;
        bool fitsFlipped = Width > box.Depth && Depth > box.Width;

        return fitsNormal || fitsFlipped;
    }
}