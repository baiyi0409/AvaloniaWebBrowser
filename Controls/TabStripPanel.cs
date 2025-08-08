using System;
using Avalonia;
using Avalonia.Controls;

namespace WebBrowserDemo.Controls;

public class TabStripPanel : Panel
{
    // 最小标签宽度
    public static readonly StyledProperty<double> MinTabWidthProperty =
        AvaloniaProperty.Register<TabStripPanel, double>(nameof(MinTabWidth), 80);

    public double MinTabWidth
    {
        get => GetValue(MinTabWidthProperty);
        set => SetValue(MinTabWidthProperty, value);
    }
    

    protected override Size MeasureOverride(Size availableSize)
    {
        double totalWidth = 0;
        double maxHeight = 0;

        // 先测量所有子项的自然宽度
        foreach (var child in Children)
        {
            child.Measure(new Size(double.PositiveInfinity, availableSize.Height));
            totalWidth += child.DesiredSize.Width;
            maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
        }

        // 如果总宽度不足，则启用压缩模式
        if (totalWidth > availableSize.Width)
        {
            double equalWidth = Math.Max(
                MinTabWidth, // 保证最小宽度
                availableSize.Width / Children.Count); // 平均分配
            
            foreach (var child in Children)
            {
                child.Measure(new Size(equalWidth, availableSize.Height));
            }
            return new Size(availableSize.Width, maxHeight);
        }

        return new Size(totalWidth, maxHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = 0;
        double itemWidth = finalSize.Width / Children.Count;
        
        foreach (var child in Children)
        {
            double width = Math.Max(itemWidth, MinTabWidth);
            child.Arrange(new Rect(x, 0, width, finalSize.Height));
            x += width;
        }
        
        return finalSize;
    }
}