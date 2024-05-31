namespace ElectroCom.Common.Controls.Buttons;

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

public class IconButton : Button
{
  static IconButton()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
  }

  public Shape Icon
  {
    get { return (Shape)GetValue(IconProperty); }
    set { SetValue(IconProperty, value); }
  }

  public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register("Icon", typeof(Shape), typeof(IconButton));

  public double IconSize
  {
    get { return (double)GetValue(IconSizeProperty); }
    set { SetValue(IconSizeProperty, value); }
  }

  public static readonly DependencyProperty IconSizeProperty =
      DependencyProperty.Register("IconSize", typeof(double), typeof(IconButton), new PropertyMetadata(16d));

  public Thickness IconMargin
  {
    get { return (Thickness)GetValue(IconMarginProperty); }
    set { SetValue(IconMarginProperty, value); }
  }

  public static readonly DependencyProperty IconMarginProperty =
      DependencyProperty.Register(
        "IconMargin",
        typeof(Thickness),
        typeof(IconButton),
        new PropertyMetadata(new Thickness(0, 0, 8, 0)));
}
