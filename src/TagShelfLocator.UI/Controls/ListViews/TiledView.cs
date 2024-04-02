namespace TagShelfLocator.UI.Controls.ListViews;

using System;
using System.Windows;
using System.Windows.Controls;

public class TiledView : ViewBase
{
  private const int defaultScale = 100;
  private const double defaultWidth = 150d;
  private const double defaultFontSize = 14d;

  protected override object DefaultStyleKey =>
    new ComponentResourceKey(this.GetType(), "TiledLayout");

  protected override object ItemContainerDefaultStyleKey =>
    new ComponentResourceKey(this.GetType(), "TagTileStyle");

  public double ItemWidth
  {
    get { return (double)GetValue(ItemWidthProperty); }
    set { SetValue(ItemWidthProperty, value); }
  }
  public static readonly DependencyProperty ItemWidthProperty =
    WrapPanel.ItemWidthProperty.AddOwner(typeof(TiledView), new PropertyMetadata(defaultWidth));

  public int ScalePercentage
  {
    get { return (int)GetValue(ScalePercentageProperty); }
    set { SetValue(ScalePercentageProperty, value); }
  }
  public static readonly DependencyProperty ScalePercentageProperty =
      DependencyProperty.Register("ScalePercentage", typeof(int), typeof(TiledView),
        new PropertyMetadata(defaultScale, OnScalePercentageChanged, CoerceScalePercentage));

  public int MinScale
  {
    get { return (int)GetValue(MinScaleProperty); }
    set { SetValue(MinScaleProperty, value); }
  }
  public static readonly DependencyProperty MinScaleProperty =
      DependencyProperty.Register("MinScale", typeof(int), typeof(TiledView), new PropertyMetadata(20));

  public int MaxScale
  {
    get { return (int)GetValue(MaxScaleProperty); }
    set { SetValue(MaxScaleProperty, value); }
  }
  public static readonly DependencyProperty MaxScaleProperty =
      DependencyProperty.Register("MaxScale", typeof(int), typeof(TiledView), new PropertyMetadata(150));

  public double FontSize
  {
    get { return (double)GetValue(FontSizeProperty); }
    set { SetValue(FontSizeProperty, value); }
  }
  public static readonly DependencyProperty FontSizeProperty =
    DependencyProperty.Register("FontSize", typeof(double), typeof(TiledView),
      new PropertyMetadata(defaultFontSize));

  private static void OnScalePercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var tiledView = (TiledView)d;

    var percentage = Convert.ToDouble((int)e.NewValue) / 100;

    ScaleItemWidth(tiledView, percentage);

    ScaleFontSize(tiledView, percentage);
  }

  private static void ScaleItemWidth(TiledView tiledView, double percentage)
  {
    var normalWidth = defaultWidth;

    var scaledWidth = percentage * normalWidth;

    tiledView.ItemWidth = scaledWidth;
  }

  private static void ScaleFontSize(TiledView tiledView, double percentage)
  {
    var normalFontSize = defaultFontSize;

    var scaledFontSize = percentage * normalFontSize;

    tiledView.FontSize = scaledFontSize;
  }

  private static object CoerceScalePercentage(DependencyObject d, object value)
  {
    TiledView tiledView = (TiledView)d;
    int currentVal = (int)value;

    currentVal = currentVal < tiledView.MinScale ? tiledView.MinScale : currentVal;
    currentVal = currentVal > tiledView.MaxScale ? tiledView.MaxScale : currentVal;

    return currentVal;
  }
}
