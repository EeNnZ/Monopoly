using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.DataAccess.Data.DataFactories;

namespace Monopoly.Warehouse.DataAccess.Data;

public static class FakeDataFactory
{
    private static readonly List<Box>    Boxes   = new BoxesDataFactory().GenerateObjects(100).Result.ToList();
    private static readonly List<Pallet> Pallets = new PalletsDataFactory().GenerateObjects(100).Result.ToList();

    public static async Task<(List<Box> boxes, List<Pallet> pallets)> GetBoxesAndPalletsAsync()
    {
        var random = new Random();
        await Task.Run(() =>
        {
            foreach (Box box in Boxes)
            {
                var tries = 0;
                while (tries <= Pallets.Count)
                {
                    Pallet randomPallet = Pallets[random.Next(Pallets.Count)];
                    if (randomPallet.BoxFits(box))
                    {
                        box.PalletId = randomPallet.Id;
                        box.Pallet = randomPallet;
                        randomPallet.Boxes.Add(box);
                        break;
                    }

                    tries++;
                }
            }
        });

        return (Boxes, Pallets);
    }
}