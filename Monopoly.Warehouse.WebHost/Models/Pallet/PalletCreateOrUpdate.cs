using System.ComponentModel.DataAnnotations;

namespace Monopoly.Warehouse.WebHost.Models.Pallet;

public class PalletCreateOrUpdate
{
    /// <summary>
    ///     Gets or sets the width of the warehouse object in appropriate units (e.g., meters or centimeters).
    /// </summary>
    [Required]
    public decimal Width { get; set; }

    /// <summary>
    ///     Gets or sets the height of the warehouse object in appropriate units (e.g., meters or centimeters).
    /// </summary>
    [Required]
    public decimal Height { get; set; }

    /// <summary>
    ///     Gets or sets the depth of the warehouse object in appropriate units (e.g., meters or centimeters).
    /// </summary>
    [Required]
    public decimal Depth { get; set; }
}