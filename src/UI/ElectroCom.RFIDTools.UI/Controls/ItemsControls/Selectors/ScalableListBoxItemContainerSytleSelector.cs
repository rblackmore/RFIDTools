#nullable disable

namespace ElectroCom.RFIDTools.UI.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
public class ScalableListBoxItemContainerStyleSelector : StyleSelector
{

  public Style ListContainerStyle { get; set; }
  public Style TileContainerStyle { get; set; }

  public override Style SelectStyle(object item, DependencyObject container)
  {
    if (item is not Layout layout)
    {
      throw new ArgumentException(("InvalidType"));
    }

    if (layout is Layout.List)
    {
      return this.ListContainerStyle;
    }
    else if (layout is Layout.Tile)
    {
      return this.TileContainerStyle;
    }

    throw new ArgumentException($"Invalid Layout {item}");
  }
}
