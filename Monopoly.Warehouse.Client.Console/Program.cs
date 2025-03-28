using System.Text;
using Monopoly.Warehouse.Client.Common;
using Monopoly.Warehouse.WebHost.Models.Box;
using Monopoly.Warehouse.WebHost.Models.Pallet;

namespace Monopoly.Warehouse.Client.Console;

internal class Program
{
    private static IEnumerable<BoxResponse>?    _boxes   = Enumerable.Empty<BoxResponse>();
    private static IEnumerable<PalletResponse>? _pallets = Enumerable.Empty<PalletResponse>();

    private static async Task Main(string[] args)
    {
        System.Console.WriteLine("Working...");
        ApiClient.Initialize();

        _pallets = await ApiClient.GetPalletsAsync();
        _boxes   = await ApiClient.GetBoxesAsync();


        string s = await PalletsGroupedAndSorted();
        System.Console.WriteLine(s);

        System.Console.ReadLine();
    }

    private static Task<string> PalletsGroupedAndSorted()
    {
        var sb = new StringBuilder();

        if (_pallets == null) return Task.FromResult("");

        var grouping = from pallet in _pallets
                       where pallet.ExpirationDate != default
                       orderby pallet.Weight descending
                       group pallet by pallet.ExpirationDate into pgroup
                       orderby pgroup.Key
                       select pgroup;

        sb.AppendLine("----------------------------ALL PALLETS GROUPED AND SORTED----------------------------");
        foreach (var g in grouping)
        {
            sb.AppendLine($"Expiration Date: ---- {g.Key:dd/MM/yyy} ----");
            sb.AppendLine("--------------------------------------------------------");
            foreach (PalletResponse? p in g)
                sb.AppendLine(
                    $"        Pallet #{p.Id} ({p.Width}x{p.Height}x{p.Depth} cm, {p.Volume} cm3, {p.Weight}kg - {p.BoxesInside} boxes inside): ");
            sb.AppendLine("--------------------------------------------------------");
            sb.AppendLine();
        }

        sb.AppendLine("----------------------------END----------------------------");


        var betterExpDatePallets = (_boxes ?? throw new InvalidOperationException())
                                  .Where(b => b.PalletId != null)
                                  .OrderByDescending(b => b.ExpirationDate)
                                  .Take(3)
                                  .Select(b => b.Pallet)
                                  .OrderBy(p => p?.Volume);

        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(
            "----------------------------3 PALLETS WITH BETTER EXPDATE SORTED BY VOLUME----------------------------");
        foreach (PalletResponse? p in betterExpDatePallets)
            if (p != null)
                sb.AppendLine(
                    $"        Pallet #{p.Id} ({p.Width}x{p.Height}x{p.Depth} cm, {p.Volume} cm3, {p.Weight}kg - {p.BoxesInside} boxes inside): ");

        return Task.FromResult(sb.ToString());
    }
}