namespace Monopoly.Warehouse.WebHost.Models.Pallet;

/// <summary>
///     Data Transfer Object for representing a Pallet.
///     This class is used to transfer pallet data between different layers of the application.
/// </summary>
public class PalletResponse : PalletShortResponse
{
    /// <summary>
    ///     Data Transfer Object for representing a Pallet.
    ///     This class is used to transfer pallet data between different layers of the application.
    /// </summary>
    public PalletResponse()
    {
    }

    /// <summary>
    ///     Data Transfer Object for representing a Pallet.
    ///     This class is used to transfer pallet data between different layers of the application.
    /// </summary>
    public PalletResponse(Warehouse.Core.Domain.Warehouse.Entities.Pallet pallet) : base(pallet)
    {
        Boxes = pallet.Boxes;
    }

    /// <summary>
    ///     Collection of boxes that are stored on this pallet.
    ///     Nullable to allow for pallets that may initially not contain any boxes.
    /// </summary>
    public ICollection<Warehouse.Core.Domain.Warehouse.Entities.Box>? Boxes { get; set; }
}