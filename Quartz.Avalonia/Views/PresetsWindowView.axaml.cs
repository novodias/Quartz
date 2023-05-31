using Avalonia.Controls;
using QuartzAvalonia.Files;
using QuartzAvalonia.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartzAvalonia.Views;

public partial class PresetsWindowView : Window
{
    public PresetsWindowView()
    {
        InitializeComponent();
    }

    public static async Task<Server?> ShowPresets(Window parent, IList<string> javaCollection, Server? preset = null)
    {
        var presetsWindow = new PresetsWindowView();
        presetsWindow.DataContext = new PresetsWindowViewModel(presetsWindow, javaCollection, preset);

        await presetsWindow.ShowDialog(parent);
        var viewModel = (presetsWindow.DataContext as PresetsWindowViewModel);

        return viewModel!.SelectedServer;
    }
}