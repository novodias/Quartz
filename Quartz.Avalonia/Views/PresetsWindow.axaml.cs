using Avalonia.Controls;
using QuartzAvalonia.Files;
using QuartzAvalonia.ViewModels;
using System.Threading.Tasks;

namespace QuartzAvalonia.Views;

public partial class PresetsView : Window
{
    public PresetsView()
    {
        InitializeComponent();
    }

    public static async Task<Server?> ShowPresets(Window parent, Server? preset = null)
    {
        var tcs = new TaskCompletionSource<Server?>();
        var presetsWindow = new PresetsView()
        {
            DataContext = new PresetsViewModel(preset)
        };

        await presetsWindow.ShowDialog(parent);
        var viewModel = (presetsWindow.DataContext as PresetsViewModel);

        return viewModel!.Server;
    }
}