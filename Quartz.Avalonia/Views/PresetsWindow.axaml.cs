using Avalonia.Controls;
using QuartzAvalonia.Files;
using QuartzAvalonia.ViewModels;
using System.Threading.Tasks;

namespace QuartzAvalonia.Views;

public partial class PresetsWindow : Window
{
    public PresetsWindow()
    {
        InitializeComponent();
    }

    public static async Task<Server?> ShowPresets(Window parent, Server? preset = null)
    {
        var presetsWindow = new PresetsWindow()
        {
            DataContext = new PresetsWindowViewModel(preset)
        };

        await presetsWindow.ShowDialog(parent);
        var viewModel = (presetsWindow.DataContext as PresetsWindowViewModel);

        return viewModel!.SelectedServer;
    }
}