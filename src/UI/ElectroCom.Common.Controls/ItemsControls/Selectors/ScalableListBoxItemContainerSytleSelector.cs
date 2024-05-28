#nullable disable

namespace ElectroCom.Common.Controls.ItemsControls.Selectors;

using System;
using System.Windows;
using System.Windows.Controls;

using ElectroCom.Common.Controls.ItemsControls;

public class ScalableListBoxItemContainerStyleSelector : StyleSelector
{

  public Style ListContainerStyle { get; set; }
  public Style TileContainerStyle { get; set; }

  public override Style SelectStyle(object item, DependencyObject container)
  {
    if (item is not Layout layout)
    {
      throw new ArgumentException("InvalidType");
    }

    if (layout is Layout.List)
    {
      return ListContainerStyle;
    }
    else if (layout is Layout.Tile)
    {
      return TileContainerStyle;
    }

    throw new ArgumentException($"Invalid Layout {item}");
  }
}
