using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace WebBrowserDemo.Controls;

public class WebTabItem:TabItem
{
    #region  删除按钮逻辑

    private ICommand? _closeTabCommand;
    public ICommand? CloseTabCommand
    {
        get => _closeTabCommand;
        set => SetAndRaise(CloseTabCommandProperty, ref _closeTabCommand, value);
    }
    public static readonly DirectProperty<WebTabItem, ICommand?> CloseTabCommandProperty =
        AvaloniaProperty.RegisterDirect<WebTabItem, ICommand?>(
            nameof(CloseTabCommand),
            o => o.CloseTabCommand,
            (o, v) => o.CloseTabCommand = v);

    #endregion
}