using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Interfaces;

namespace Monopoly.API.Services.Pallets;

public class PalletsGeneratorService : IDataGenerator<Pallet>
{
    public Task<IEnumerable<Pallet>> GenerateObjects(int count)
    {
        var random           = new Random();
        var generatedPallets = new List<Pallet>();
        for (var i = 0; i < count; i++)
            generatedPallets.Add(new Pallet
            {
                Width  = Convert.ToDecimal(random.Next(100, 300)),
                Height = Convert.ToDecimal(random.Next(100, 300)),
                Depth  = Convert.ToDecimal(random.Next(100, 400))
            });
        return Task.FromResult<IEnumerable<Pallet>>(generatedPallets);
    }
}