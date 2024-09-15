using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Monopoly.API.Data.DTOs;
using NuGet.Packaging;

namespace Monopoly.WpfClient;

public partial class MainViewModel : ObservableObject
{
    private readonly             Dispatcher _dispatcher;


    [ObservableProperty] private DateOnly   _dateCreated;
    [ObservableProperty] private int        _depth;
    [ObservableProperty] private DateOnly   _expirationDate;
    [ObservableProperty] private int        _height;


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

    public ObservableCollection<BoxDto>    Boxes   { get; set; } = new();
    public ObservableCollection<PalletDto> Pallets { get; set; } = new();

    public ObservableCollection<string> BoxDtoProperties { get; set; } =
        new(typeof(BoxDto).GetProperties().Select(pi => pi.Name));

    public ObservableCollection<string> PalletDtoProperties { get; set; } =
        new(typeof(PalletDto).GetProperties().Select(pi => pi.Name));

    [RelayCommand]
    private async Task FillCollections()
    {
        try
        {
            var getBoxesTask = Task.Run(async () =>
            {
                await _dispatcher.InvokeAsync(
                    async () => Boxes.AddRange(await ApiClient.GetBoxesAsync()));
            });
            var getPalletsTask = Task.Run(async () =>
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