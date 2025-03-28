using System.ComponentModel.DataAnnotations;

namespace Monopoly.Warehouse.WebHost.Models.Box;

public class BoxCreateOrUpdate
{
    private DateOnly? _expirationDate;

    public BoxCreateOrUpdate(decimal   weight,
                             DateOnly? dateCreated    = null,
                             DateOnly? expirationDate = null)
    {
        Weight = weight;

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

    [Required] public decimal Weight { get; }

    public DateOnly? DateCreated { get; set; }

    public DateOnly? ExpirationDate
    {
        get => DateCreated?.AddDays(100) ?? _expirationDate;
        set
        {
            if (DateCreated == null)
                _expirationDate = value;
        }
    }
}