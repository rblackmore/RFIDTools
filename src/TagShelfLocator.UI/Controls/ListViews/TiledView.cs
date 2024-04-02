namespace TagShelfLocator.UI.Controls.ListViews;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

public class TiledView : ViewBase
{
  protected override object DefaultStyleKey =>
    new ComponentResourceKey(this.GetType(), "TiledLayout");

  protected override object ItemContainerDefaultStyleKey =>
    new ComponentResourceKey(this.GetType(), "TagTileStyle");

  public static readonly DependencyProperty ColumnsProperty =
    UniformGrid.ColumnsProperty.AddOwner(typeof(TiledView));
  public int Columns
  {
    get => (int)GetValue(ColumnsProperty);
    set => SetValue(ColumnsProperty, value);
  }

  public double ItemWidth
  {
    get { return (double)GetValue(ItemWidthProperty); }
    set { SetValue(ItemWidthProperty, value); }
  }

  public static readonly DependencyProperty ItemWidthProperty =
    WrapPanel.ItemWidthProperty.AddOwner(typeof(TiledView));
}
