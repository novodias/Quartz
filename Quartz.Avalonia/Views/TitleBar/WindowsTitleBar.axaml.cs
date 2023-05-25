using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace QuartzAvalonia.Views.TitleBar;

public partial class WindowsTitleBar : UserControl
{
    private Button minimizeButton;
    private Button maximizeButton;
    private Path maximizeIcon;
    private ToolTip maximizeToolTip;
    private Button closeButton;
    private Image windowIcon;

    private DockPanel titleBar;
    private DockPanel titleBarBackground;
    private TextBlock systemChromeTitle;

    private bool _mouseDownForWindowMoving = false;
    private PointerPoint _originalPoint;

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<WindowsTitleBar, string>(nameof(Title));
    
    public string Title
    {
        get { return GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public WindowsTitleBar()
    {
        InitializeComponent();

        minimizeButton = this.FindControl<Button>("MinimizeButton");
        maximizeButton = this.FindControl<Button>("MaximizeButton");
        maximizeIcon = this.FindControl<Path>("MaximizeIcon");
        maximizeToolTip = this.FindControl<ToolTip>("MaximizeToolTip");
        closeButton = this.FindControl<Button>("CloseButton");
        windowIcon = this.FindControl<Image>("WindowIcon");

        minimizeButton.Click += MinimizeWindow;
        maximizeButton.Click += MaximizeWindow;
        closeButton.Click += CloseWindow;
        windowIcon.DoubleTapped += CloseWindow;

        titleBar = this.FindControl<DockPanel>("TitleBar");
        titleBarBackground = this.FindControl<DockPanel>("TitleBarBackground");
        systemChromeTitle = this.FindControl<TextBlock>("SystemChromeTitle");

        titleBarBackground.PointerMoved += InputElement_OnPointerMoved;
        titleBarBackground.PointerPressed += InputElement_OnPointerPressed;
        titleBarBackground.PointerReleased += InputElement_OnPointerReleased;

        if (Title == null)
        {
            Title = "Quartz";
        }

        Dispatcher.UIThread.Post(() => systemChromeTitle.Text = Title);

        SubscribeToWindowState();
    }

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_mouseDownForWindowMoving) return;
        Window hostWindow = (Window)this.VisualRoot!;

        PointerPoint currentPoint = e.GetCurrentPoint(this);
        hostWindow.Position = new PixelPoint(hostWindow.Position.X + (int)(currentPoint.Position.X - _originalPoint.Position.X),
            hostWindow.Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Window hostWindow = (Window)this.VisualRoot!;
        if (hostWindow.WindowState == WindowState.Maximized || hostWindow.WindowState == WindowState.FullScreen) return;

        _mouseDownForWindowMoving = true;
        _originalPoint = e.GetCurrentPoint(this);
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _mouseDownForWindowMoving = false;
    }

    private void CloseWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Window hostWindow = (Window)this.VisualRoot!;
        hostWindow?.Close();
    }

    private void MaximizeWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Window hostWindow = (Window)this.VisualRoot!;

        if (hostWindow.WindowState == WindowState.Normal)
        {
            hostWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            hostWindow.WindowState = WindowState.Normal;
        }
    }

    private void MinimizeWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Window hostWindow = (Window)this.VisualRoot;
        hostWindow.WindowState = WindowState.Minimized;
    }

    private async void SubscribeToWindowState()
    {
        Window hostWindow = (Window)this.VisualRoot;

        while (hostWindow == null)
        {
            hostWindow = (Window)this.VisualRoot;
            await Task.Delay(50);
        }

        hostWindow.GetBindingObservable(Window.WindowStateProperty).Subscribe(s =>
        {
            if (!s.HasValue)
            {
                return;
            }

            if (s.Value != WindowState.Maximized)
            {
                maximizeIcon.Data = Geometry.Parse("M2048 2048v-2048h-2048v2048h2048zM1843 1843h-1638v-1638h1638v1638z");
                hostWindow.Padding = new Thickness(0, 0, 0, 0);
                maximizeToolTip.Content = "Maximize";
            }
            if (s.Value == WindowState.Maximized)
            {
                maximizeIcon.Data = Geometry.Parse("M2048 1638h-410v410h-1638v-1638h410v-410h1638v1638zm-614-1024h-1229v1229h1229v-1229zm409-409h-1229v205h1024v1024h205v-1229z");
                hostWindow.Padding = new Thickness(7, 7, 7, 7);
                maximizeToolTip.Content = "Restore Down";
            }
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}