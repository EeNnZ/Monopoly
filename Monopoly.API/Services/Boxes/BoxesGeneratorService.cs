using Microsoft.EntityFrameworkCore;
using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Interfaces;

namespace Monopoly.API.Services.Boxes;

public class BoxesGeneratorService(MainDbContext context) : IDataGenerator<Box>
{
    public async Task<IEnumerable<Box>> GenerateObjects(int count)
    {
        var random         = new Random();
        var generatedBoxes = new List<Box>();

        for (var i = 0; i < count; i++)
            generatedBoxes.Add(new Box(Convert.ToDecimal(random.Next(1, 15)), DateOnly.FromDateTime(DateTime.Today))
            {
                Width       = Convert.ToDecimal(random.Next(100, 400)),
                Height      = Convert.ToDecimal(random.Next(100, 300)),
                Depth       = Convert.ToDecimal(random.Next(100, 500)),
                DateCreated = new DateOnly(DateTime.Today.Year, random.Next(3,  6), random.Next(1, 30))
            });

        if (context.Pallets == null)
            throw new NullReferenceException(nameof(context.Pallets));

        if (context.Pallets == null)
            throw new NullReferenceException(nameof(context.Boxes));

        var pallets = await context.Pallets.ToListAsync();

        foreach (var box in generatedBoxes)
        {
            var tries = 0;
            while (tries <= pallets.Count)
            {
                var randomPallet = pallets[random.Next(pallets.Count)];
                if (randomPallet.BoxFits(box))
                {
                    box.Pallet = randomPallet;
                    break;
                }

                tries++;
            }
        }

        return generatedBoxes;
    }
}