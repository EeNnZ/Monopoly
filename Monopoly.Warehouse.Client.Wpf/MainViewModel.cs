using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Monopoly.Warehouse.Client.Common;
using Monopoly.Warehouse.WebHost.Models.Box;
using Monopoly.Warehouse.WebHost.Models.Pallet;
using NuGet.Packaging;

namespace Monopoly.Warehouse.Client.Wpf;

public partial class MainViewModel : ObservableObject
{
    private readonly Dispatcher _dispatcher;


    [ObservableProperty] private DateOnly _dateCreated;
    [ObservableProperty] private int      _depth;
    [ObservableProperty] private DateOnly _expirationDate;
    [ObservableProperty] private int      _height;


    [ObservableProperty] private int _id;
    [ObservableProperty] private int _palletId;

    [ObservableProperty] private string _selectedBoxGroupProperty;
    [ObservableProperty] private string _selectedPalletGroupProperty;
    [ObservableProperty] private int    _volume;
    [ObservableProperty] private int    _weight;
    [ObservableProperty] private int    _width;

    public MainViewModel()
    {
        ApiClient.Initialize();
        _dispatcher = Dispatcher.CurrentDispatcher;
    }

    public ObservableCollection<BoxResponse>    Boxes   { get; set; } = new();
    public ObservableCollection<PalletResponse> Pallets { get; set; } = new();

    public ObservableCollection<string> BoxDtoProperties { get; set; } =
        new(typeof(BoxResponse).GetProperties().Select(pi => pi.Name));

    public ObservableCollection<string> PalletDtoProperties { get; set; } =
        new(typeof(PalletResponse).GetProperties().Select(pi => pi.Name));

    [RelayCommand]
    private async Task FillCollections()
    {
        try
        {
            Task getBoxesTask = Task.Run(async () =>
            {
                await _dispatcher.InvokeAsync(
                    async () => Boxes.AddRange(await ApiClient.GetBoxesAsync()));
            });
            Task getPalletsTask = Task.Run(async () =>
            {
                await _dispatcher.InvokeAsync(
                    async () => Pallets.AddRange(await ApiClient.GetPalletsAsync()));
            });

            await Task.WhenAll(getBoxesTask, getPalletsTask);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    [RelayCommand]
    private async Task ExecuteBoxes()
    {
        throw new NotImplementedException();
    }

    [RelayCommand]
    private async Task ExecutePallets()
    {
        throw new NotSupportedException();
    }
}