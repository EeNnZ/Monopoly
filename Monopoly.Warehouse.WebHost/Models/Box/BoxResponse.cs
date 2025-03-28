using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.WebHost.Models.Pallet;

namespace Monopoly.Warehouse.WebHost.Models.Box;

/// <summary>
///     Data Transfer Object (DTO) representing a box with various properties.
///     This class is used to transfer box data between different layers of the application,
///     such as between the data access layer and the presentation layer.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="BoxResponse" /> class.
///     This constructor takes a <see cref="Box" /> object and maps its properties to the DTO.
/// </remarks>
public class BoxResponse : BoxShortResponse
{
    /// <summary>
    ///     Data Transfer Object (DTO) representing a box with various properties.
    ///     This class is used to transfer box data between different layers of the application,
    ///     such as between the data access layer and the presentation layer.
    /// </summary>
    /// <remarks>
    ///     Initializes a new instance of the <see cref="BoxResponse" /> class.
    /// </remarks>
    public BoxResponse()
    {
    }

    /// <summary>
    ///     Data Transfer Object (DTO) representing a box with various properties.
    ///     This class is used to transfer box data between different layers of the application,
    ///     such as between the data access layer and the presentation layer.
    /// </summary>
    /// <remarks>
    ///     Initializes a new instance of the <see cref="BoxResponse" /> class.
    ///     This constructor takes a <see cref="Box" /> object and maps its properties to the DTO.
    /// </remarks>
    /// <param name="box">The original Box object to map data from.</param>
    public BoxResponse(Warehouse.Core.Domain.Warehouse.Entities.Box box)
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
        Pallet         = box.Pallet is null ? null : new PalletResponse(box.Pallet);
    }


    /// <summary>
    ///     Gets or sets the pallet box associated with.
    ///     This property can be null if the box is not associated with any pallet.
    /// </summary>
    public PalletResponse? Pallet { get; set; }
}