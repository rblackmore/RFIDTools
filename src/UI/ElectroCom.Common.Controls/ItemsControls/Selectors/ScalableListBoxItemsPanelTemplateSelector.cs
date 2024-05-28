#nullable disable
namespace ElectroCom.Common.Controls.ItemsControls.Selectors;

using System;
using System.Windows;
using System.Windows.Controls;

using ElectroCom.Common.Controls.Selectors;

public class ScalableListBoxItemsPanelTemplateSelector : ItemsPanelTemplateSelector
{
  public ItemsPanelTemplate ListPanelTemplate { get; set; }
  public ItemsPanelTemplate TilePanelTemplate { get; set; }

  public override ItemsPanelTemplate SelectTemplate(object item, DependencyObject container)
  {
    if (item is not Layout layout)
    {
      throw new ArgumentException("InvalidType");
    }

    if (layout is Layout.List)
    {
      return ListPanelTemplate;
    }
    else if (layout is Layout.Tile)
    {
      return TilePanelTemplate;
    }

    throw new ArgumentException($"Invalid Layout {item}");
  }
}
