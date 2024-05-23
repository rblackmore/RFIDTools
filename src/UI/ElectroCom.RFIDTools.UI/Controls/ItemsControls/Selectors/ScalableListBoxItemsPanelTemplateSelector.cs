#nullable disable
namespace ElectroCom.RFIDTools.UI.Controls;

using System;
using System.Windows;
using System.Windows.Controls;

using ElectroCom.RFIDTools.UI.Controls.Selectors;

public class ScalableListBoxItemsPanelTemplateSelector : ItemsPanelTemplateSelector
{
  public ItemsPanelTemplate ListPanelTemplate { get; set; }
  public ItemsPanelTemplate TilePanelTemplate { get; set; }

  public override ItemsPanelTemplate SelectTemplate(object item, DependencyObject container)
  {
    if (item is not Layout layout)
    {
      throw new ArgumentException(("InvalidType"));
    }

    if (layout is Layout.List)
    {
      return this.ListPanelTemplate;
    }
    else if (layout is Layout.Tile)
    {
      return this.TilePanelTemplate;
    }

    throw new ArgumentException($"Invalid Layout {item}");
  }
}
