using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

namespace Monopoly.Warehouse.DataAccess.Data.DataFactories;

public class BoxesDataFactory : IDataFactory<Box>
{
    public Task<IEnumerable<Box>> GenerateObjects(int count)
    {
        var random         = new Random();
        var generatedBoxes = new List<Box>();

        for (var i = 0; i < count; i++)
            generatedBoxes.Add(new Box()
            {
                Weight      = Convert.ToDecimal(random.Next(1,                 15)),
                Width       = Convert.ToDecimal(random.Next(100,               400)),
                Height      = Convert.ToDecimal(random.Next(100,               300)),
                Depth       = Convert.ToDecimal(random.Next(100,               500)),
                DateCreated = new DateOnly(DateTime.Today.Year, random.Next(3, 6), random.Next(1, 30))
            });
        return Task.FromResult(generatedBoxes.AsEnumerable());
    }
}