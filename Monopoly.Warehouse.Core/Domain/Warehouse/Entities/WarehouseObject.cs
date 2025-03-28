namespace Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

/// <summary>
///     Represents an abstract base class for warehouse objects.
///     This class serves as a foundation for specific types of warehouse items, defining common properties and behaviors.
/// </summary>
public abstract class WarehouseObject : BaseEntity
{
    /// <summary>
    ///     Gets or sets the width of the warehouse object in appropriate units (e.g., meters or centimeters).
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    ///     Gets or sets the height of the warehouse object in appropriate units (e.g., meters or centimeters).
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    ///     Gets or sets the depth of the warehouse object in appropriate units (e.g., meters or centimeters).
    /// </summary>
    public decimal Depth { get; set; }

    /// <summary>
    ///     Gets the weight of the warehouse object in appropriate units (e.g., kilograms or pounds).
    ///     This property must be implemented in derived classes.
    /// </summary>
    public virtual decimal Weight { get; set; }

    /// <summary>
    ///     Gets the volume of the warehouse object, calculated from its dimensions.
    ///     This property must be implemented in derived classes.
    /// </summary>
    public abstract decimal Volume { get; }
}