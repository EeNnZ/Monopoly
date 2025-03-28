using Microsoft.EntityFrameworkCore;
using Monopoly.Warehouse.DataAccess.Data;

namespace Monopoly.Warehouse.DataAccess;

public static class DbInitializer
{
    public static async void Initialize(DataContext context)
    {
        if (!await context.Boxes.AnyAsync())
        {
            var data = await FakeDataFactory.GetBoxesAndPalletsAsync();
            
            await context.Boxes.AddRangeAsync(data.boxes);
            await context.Pallets.AddRangeAsync(data.pallets);
        }
        
        await context.SaveChangesAsync();
    }
}