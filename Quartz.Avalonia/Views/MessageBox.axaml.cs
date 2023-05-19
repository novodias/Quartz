using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace QuartzAvalonia.Views;

public partial class MessageBox : Window
{
    public enum MessageBoxButtons
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel
    }

    public enum MessageBoxResult
    {
        Ok,
        Cancel,
        Yes,
        No
    }

    public MessageBox()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static Task<MessageBoxResult> Show(Window parent, string text, string title, MessageBoxButtons buttons)
    {
        var box = new MessageBox()
        {
            Title = title,
            SizeToContent = SizeToContent.WidthAndHeight,
            CanResize = false,
        };

        box.FindControl<TextBlock>("TextBlock").Text = text;
        var buttonsPanel = box.FindControl<StackPanel>("Buttons");
        var result = MessageBoxResult.Ok;
        void AddButton(string caption, MessageBoxResult r, bool def = false)
        {
            var btn = new Button()
            {
                Content = caption,
                Padding = new Thickness(10),
                MinWidth = 80,
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };

            btn.Click += (_, __) => {
                result = r;
                box!.Close();
            };

            buttonsPanel.Children.Add(btn);
        }

        if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
            AddButton("Ok", MessageBoxResult.Ok, true);
        if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
        {
            AddButton("Yes", MessageBoxResult.Yes);
            AddButton("No", MessageBoxResult.No, true);
        }

        if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
            AddButton("Cancel", MessageBoxResult.Cancel, true);

        var tcs = new TaskCompletionSource<MessageBoxResult>();
        box.Closed += delegate { tcs.TrySetResult(result); };
        if (parent != null) box.ShowDialog(parent);
        else box.Show();
        return tcs.Task;
    }
}