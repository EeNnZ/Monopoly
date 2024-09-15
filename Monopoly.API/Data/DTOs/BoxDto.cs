using Monopoly.API.Data.Entities;

namespace Monopoly.API.Data.DTOs;

/// <summary>
///     Data Transfer Object (DTO) representing a box with various properties.
///     This class is used to transfer box data between different layers of the application,
///     such as between the data access layer and the presentation layer.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="BoxDto" /> class.
///     This constructor takes a <see cref="Box" /> object and maps its properties to the DTO.
/// </remarks>
public class BoxDto
{
    /// <summary>
    ///     Data Transfer Object (DTO) representing a box with various properties.
    ///     This class is used to transfer box data between different layers of the application,
    ///     such as between the data access layer and the presentation layer.
    /// </summary>
    /// <remarks>
    ///     Initializes a new instance of the <see cref="BoxDto" /> class.
    /// </remarks>
    public BoxDto()
    {
    }

    /// <summary>
    ///     Data Transfer Object (DTO) representing a box with various properties.
    ///     This class is used to transfer box data between different layers of the application,
    ///     such as between the data access layer and the presentation layer.
    /// </summary>
    /// <remarks>
    ///     Initializes a new instance of the <see cref="BoxDto" /> class.
    ///     This constructor takes a <see cref="Box" /> object and maps its properties to the DTO.
    /// </remarks>
    /// <param name="box">The original Box object to map data from.</param>
    public BoxDto(Box box)
    {
        Id             = box.Id;
        Width          = box.Width;
        Height         = box.Height;
        Depth          = box.Depth;
        Weight         = box.Weight;
        Volume         = box.Volume;
        ExpirationDate = box.ExpirationDate;
        DateCreated    = box.DateCreated;
        PalletId       = box.PalletId;
        Pallet         = box.Pallet is null ? null : new PalletDto(box.Pallet);
    }

    /// <summary>
    ///     Gets or sets the unique identifier for the box.
    /// </summary>
    public long Id { get; set; }

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
    public long? PalletId { get; set; }

    /// <summary>
    ///     Gets or sets the pallet box associated with.
    ///     This property can be null if the box is not associated with any pallet.
    /// </summary>
    public PalletDto? Pallet { get; set; }
}