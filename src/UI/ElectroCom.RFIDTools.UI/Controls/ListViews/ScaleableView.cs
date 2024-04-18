namespace ElectroCom.RFIDTools.UI.Controls.ListViews;

using System;
using System.Windows;
using System.Windows.Controls;

public class ScaleableView : ViewBase
{
  private const int defaultScale = 100;
  private const double defaultWidth = 150d;
  private const double defaultHeight = 48d;
  private const double defaultFontSize = 12d;

  #region Dependency_Properties
  public static readonly DependencyProperty ItemWidthProperty =
    WrapPanel.ItemWidthProperty.AddOwner(typeof(ScaleableView), new PropertyMetadata(defaultWidth));

  public static readonly DependencyProperty ItemHeightProperty =
    DependencyProperty.Register("ItemHeight", typeof(double), typeof(ScaleableView),
      new PropertyMetadata(defaultHeight));

  public static readonly DependencyProperty ScalePercentageProperty =
      DependencyProperty.Register("ScalePercentage", typeof(int), typeof(ScaleableView),
        new PropertyMetadata(defaultScale, OnScalePercentageChanged, CoerceScalePercentage));

  public static readonly DependencyProperty MinScaleProperty =
      DependencyProperty.Register("MinScale", typeof(int), typeof(ScaleableView), new PropertyMetadata(20));

  public static readonly DependencyProperty MaxScaleProperty =
      DependencyProperty.Register("MaxScale", typeof(int), typeof(ScaleableView), new PropertyMetadata(150));

  public static readonly DependencyProperty FontSizeProperty =
    DependencyProperty.Register("FontSize", typeof(double), typeof(ScaleableView),
      new PropertyMetadata(defaultFontSize));
  #endregion

  #region Properties

  public double ItemWidth
  {
    get { return (double)GetValue(ItemWidthProperty); }
    set { SetValue(ItemWidthProperty, value); }
  }

  public double ItemHeight
  {
    get { return (double)GetValue(ItemHeightProperty); }
    set { SetValue(ItemHeightProperty, value); }
  }

  public int ScalePercentage
  {
    get { return (int)GetValue(ScalePercentageProperty); }
    set { SetValue(ScalePercentageProperty, value); }
  }
  public int MinScale
  {
    get { return (int)GetValue(MinScaleProperty); }
    set { SetValue(MinScaleProperty, value); }
  }
  public int MaxScale
  {
    get { return (int)GetValue(MaxScaleProperty); }
    set { SetValue(MaxScaleProperty, value); }
  }
  public double FontSize
  {
    get { return (double)GetValue(FontSizeProperty); }
    set { SetValue(FontSizeProperty, value); }
  }
  #endregion

  private static void OnScalePercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var tiledView = (ScaleableView)d;

    var percentage = Convert.ToDouble((int)e.NewValue) / 100;

    ScaleItemWidth(tiledView, percentage);

    ScaleItemHeight(tiledView, percentage);

    ScaleFontSize(tiledView, percentage);
  }

  private static void ScaleItemWidth(ScaleableView tiledView, double percentage)
  {
    var normalWidth = defaultWidth;

    var scaledWidth = percentage * normalWidth;

    tiledView.ItemWidth = scaledWidth;
  }

  private static void ScaleItemHeight(ScaleableView tiledView, double percentage)
  {
    var normalHeight = defaultHeight;

    var scaledHeight = percentage * normalHeight;

    tiledView.ItemHeight = scaledHeight;
  }

  private static void ScaleFontSize(ScaleableView tiledView, double percentage)
  {
    var normalFontSize = defaultFontSize;

    var scaledFontSize = percentage * normalFontSize;

    tiledView.FontSize = scaledFontSize;
  }

  private static object CoerceScalePercentage(DependencyObject d, object value)
  {
    var tiledView = (ScaleableView)d;
    var currentVal = (int)value;

    currentVal = currentVal < tiledView.MinScale ? tiledView.MinScale : currentVal;
    currentVal = currentVal > tiledView.MaxScale ? tiledView.MaxScale : currentVal;

    return currentVal;
  }
}
