using Monopoly.API.Data.Entities;

namespace Monopoly.API.Data.DTOs;

/// <summary>
///     Data Transfer Object for representing a Pallet.
///     This class is used to transfer pallet data between different layers of the application.
/// </summary>
public class PalletDto
{
    /// <summary>
    ///     Data Transfer Object for representing a Pallet.
    ///     This class is used to transfer pallet data between different layers of the application.
    /// </summary>
    public PalletDto()
    {
    }

    /// <summary>
    ///     Data Transfer Object for representing a Pallet.
    ///     This class is used to transfer pallet data between different layers of the application.
    /// </summary>
    public PalletDto(Pallet pallet)
    {
        Id             = pallet.Id;
        Width          = pallet.Width;
        Height         = pallet.Height;
        Depth          = pallet.Depth;
        Weight         = pallet.Weight;
        Volume         = pallet.Volume;
        ExpirationDate = pallet.ExpirationDate.HasValue ? pallet.ExpirationDate.Value.ToShortDateString() : "No";
        BoxesInside    = pallet.Boxes?.Count ?? 0;
    }

    /// <summary>
    ///     Unique identifier for the pallet
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     Width of the pallet
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    ///     Height of the pallet
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    ///     Depth of the pallet
    /// </summary>
    public decimal Depth { get; set; }

    /// <summary>
    ///     Weight of the pallet
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    ///     Volume of the pallet
    /// </summary>
    public decimal Volume { get; set; }

    /// <summary>
    ///     Expiration date of the pallet contents formatted as a string
    /// </summary>
    public string? ExpirationDate { get; set; }

    /// <summary>
    ///     Number of boxes inside the pallet, defaults to 0 if no boxes exist
    /// </summary>
    public int BoxesInside { get; set; } // Uses null-conditional operator to safely count boxes if they exist
}