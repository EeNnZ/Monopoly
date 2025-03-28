namespace Monopoly.Warehouse.WebHost.Models.Box;

public class BoxShortResponse
{
    /// <summary>
    ///     Gets or sets the unique identifier for the box.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the width of the box in decimal units.
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    ///     Gets or sets the height of the box in decimal units.
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    ///     Gets or sets the depth of the box in decimal units.
    /// </summary>
    public decimal Depth { get; set; }

    /// <summary>
    ///     Gets or sets the weight of the box in decimal units.
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    ///     Gets or sets the volume of the box in decimal units.
    /// </summary>
    public decimal Volume { get; set; }

    /// <summary>
    ///     Gets or sets the expiration date of the box as a short date string.
    /// </summary>
    public DateOnly? ExpirationDate { get; set; }

    /// <summary>
    ///     Gets or sets the creation date of the box as a short date string.
    /// </summary>
    public DateOnly? DateCreated { get; set; }

    /// <summary>
    ///     Gets or sets the optional identifier for a pallet that the box may belong to.
    ///     This property can be null if the box is not associated with any pallet.
    /// </summary>
    public Guid? PalletId { get; set; }
}