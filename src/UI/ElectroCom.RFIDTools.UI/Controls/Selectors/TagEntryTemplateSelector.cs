namespace TagShelfLocator.UI.Controls.Selectors;

using System.Windows;
using System.Windows.Controls;

using TagShelfLocator.UI.MVVM.Modal;

public class TagEntryTemplateSelector : DataTemplateSelector
{
  public DataTemplate? Default { get; set; }
  public DataTemplate? EPCC1G2_Template { get; set; }
  public DataTemplate? ISO14443A_Template { get; set; }
  public DataTemplate? ISO15693_Template { get; set; }

  public override DataTemplate? SelectTemplate(object item, DependencyObject container)
  {
    FrameworkElement? element = container as FrameworkElement;

    if (element is null || item is null || item is not TagEntry tagEntry) return Default;

    if (tagEntry is EPCClass1Gen2_TagEntry)
      return EPCC1G2_Template;

    if (tagEntry is ISO14443A_TagEntry)
      return ISO14443A_Template;

    if (tagEntry is ISO15693_TagEntry)
      return ISO15693_Template;

    return Default;
  }
}
