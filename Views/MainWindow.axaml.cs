using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using WebBrowserDemo.Controls;
using WebBrowserDemo.Extensions;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Handlers;

namespace WebBrowserDemo.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeChromium();
    }

    private void InitializeChromium()
    {
        WebTabControl.AddTabCommand = new RelayCommand(() =>
        {
            WebTabItem tabItem = new WebTabItem();
            WebTabControl.Items.Add(tabItem);
            
            var browser = new AvaloniaCefBrowser();
            browser.Address = "https://www.baidu.com";
            browser.TitleChanged += (sender, title) =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    tabItem.Header = title;
                });
            };
            browser.LifeSpanHandler = new WebLifeSpanHandler();
            tabItem.Header = $"新建标签页";
            tabItem.Content = browser;
            WebTabControl.SelectedItem = tabItem;
        });
    }

    private void ClosButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void MaxButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (this.WindowState != WindowState.FullScreen)
        {
            this.WindowState = this.WindowState == WindowState.Maximized 
                ? WindowState.Normal 
                : WindowState.Maximized;
        }
    }


    private void MinButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }
    
    private void FullScreenButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.WindowState = this.WindowState == WindowState.FullScreen 
            ? WindowState.Normal 
            : WindowState.FullScreen;
    }

    private void ThemeButton_OnClick(object? sender, RoutedEventArgs e)
    {
        App.Current.RequestedThemeVariant = 
            App.Current.RequestedThemeVariant == ThemeVariant.Light
                ? ThemeVariant.Dark 
                : ThemeVariant.Light;
    }


    private void DragWindow(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            // 获取窗口引用
            if (VisualRoot is Window window)
            {
                window.BeginMoveDrag(e);
            }
        }
    }
}