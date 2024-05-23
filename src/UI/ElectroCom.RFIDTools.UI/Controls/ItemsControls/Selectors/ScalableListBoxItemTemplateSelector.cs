#nullable disable

namespace ElectroCom.RFIDTools.UI.Controls;

using System;
using System.Windows;
using System.Windows.Controls;

public class ScalableListBoxItemTemplateSelector : DataTemplateSelector
{
  public DataTemplate ListItemTemplate { get; set; }
  public DataTemplate TileItemTemplate { get; set; }

  public override DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    if (item is not Layout layout)
    {
      throw new ArgumentException(("InvalidType"));
    }

    if (layout is Layout.List)
    {
      return this.ListItemTemplate;
    }
    else if (layout is Layout.Tile)
    {
      return this.TileItemTemplate;
    }

    throw new ArgumentException($"Invalid Layout {item}");
  }
}
