#nullable disable

namespace ElectroCom.Common.Controls.ItemsControls.Selectors;

using System;
using System.Windows;
using System.Windows.Controls;

using ElectroCom.Common.Controls.ItemsControls;

public class ScalableListBoxItemTemplateSelector : DataTemplateSelector
{
  public DataTemplate ListItemTemplate { get; set; }
  public DataTemplate TileItemTemplate { get; set; }

  public override DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    if (item is not Layout layout)
    {
      throw new ArgumentException("InvalidType");
    }

    if (layout is Layout.List)
    {
      return ListItemTemplate;
    }
    else if (layout is Layout.Tile)
    {
      return TileItemTemplate;
    }

    throw new ArgumentException($"Invalid Layout {item}");
  }
}
