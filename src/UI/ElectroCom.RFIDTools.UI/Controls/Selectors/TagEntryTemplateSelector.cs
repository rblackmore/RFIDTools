namespace ElectroCom.RFIDTools.UI.Controls.Selectors;

using System.Windows;
using System.Windows.Controls;

using ElectroCom.RFIDTools.UI.Logic.ViewModels;

public class TagEntryTemplateSelector : DataTemplateSelector
{
  public DataTemplate? Default { get; set; }
  public DataTemplate? EPCC1G2_Template { get; set; }
  public DataTemplate? ISO14443A_Template { get; set; }
  public DataTemplate? ISO15693_Template { get; set; }

  public override DataTemplate? SelectTemplate(object item, DependencyObject container)
  {
    FrameworkElement? element = container as FrameworkElement;

    if (element is null || item is null || item is not ObservableTagEntry tagEntry) return Default;

    // TODO: Should not longer check subtype of tagEntry.
    // Instead will have a property or methods to check tag type, like FEDM.TagItem has eg. 'isEPCClass1Gen2()'

    //if (tagEntry is EPCClass1Gen2_TagEntry)
    //  return EPCC1G2_Template;

    //if (tagEntry is ISO14443A_TagEntry)
    //  return ISO14443A_Template;

    //if (tagEntry is ISO15693_TagEntry)
    //  return ISO15693_Template;

    return Default;
  }
}
