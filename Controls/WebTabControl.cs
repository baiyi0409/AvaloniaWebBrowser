using System;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.Input;

namespace WebBrowserDemo.Controls;

public class WebTabControl:TabControl
{
    #region 按钮组

    private object? _buttonContent;
    
    public object? ButtonContent
    {
        get => _buttonContent;
        internal set => SetAndRaise(SelectedContentProperty, ref _buttonContent, value);
    }
    
    public static readonly DirectProperty<WebTabControl, object?> ButtonContentProperty =
        AvaloniaProperty.RegisterDirect<WebTabControl, object?>(nameof(ButtonContent), o => o.ButtonContent);
    
    #endregion

    #region 是否显示添加按钮
    
    private bool _showAddNewPage = false;
    public bool ShowAddNewPage
    {
        get => _showAddNewPage;
        set => SetAndRaise(ShowAddNewPageProperty, ref _showAddNewPage, value);
    }
    
    public static readonly DirectProperty<WebTabControl, bool> ShowAddNewPageProperty =
        AvaloniaProperty.RegisterDirect<WebTabControl, bool>(
            nameof(ShowAddNewPage),
            o => o.ShowAddNewPage,
            (o, v) => o.ShowAddNewPage = v);

    #endregion

    #region  添加按钮逻辑

    private ICommand? _addTabCommand;
    public ICommand? AddTabCommand
    {
        get => _addTabCommand;
        set => SetAndRaise(AddTabCommandProperty, ref _addTabCommand, value);
    }
    public static readonly DirectProperty<WebTabControl, ICommand?> AddTabCommandProperty =
        AvaloniaProperty.RegisterDirect<WebTabControl, ICommand?>(
            nameof(AddTabCommand),
            o => o.AddTabCommand,
            (o, v) => o.AddTabCommand = v);

    #endregion

    #region 删除子项逻辑

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        // 初始化时给所有子项设置关闭命令
        foreach (var item in Items.OfType<WebTabItem>())
        {
            item.CloseTabCommand = new RelayCommand(() => CloseTab(item));
        }
        
        // 监听集合变化
        Items.CollectionChanged += (s, e) => 
        {
            if (e.NewItems != null)
            {
                foreach (WebTabItem newItem in e.NewItems.OfType<WebTabItem>())
                {
                    newItem.CloseTabCommand = new RelayCommand(() => CloseTab(newItem));
                }
            }
        };
    }

    private void CloseTab(WebTabItem tab)
    {
        // 确保至少保留一个标签页
        if (Items.Count <= 1) return;
        
        Items.Remove(tab);
        
        // 如果移除的是当前选中项，自动选中前一个
        if (SelectedItem == tab)
        {
            SelectedIndex = Math.Max(0, Items.Count - 1);
        }
    }

    #endregion
}