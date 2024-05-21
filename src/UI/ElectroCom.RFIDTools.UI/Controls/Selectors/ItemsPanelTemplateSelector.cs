namespace ElectroCom.RFIDTools.UI.Controls.Selectors;

using System.Windows;
using System.Windows.Controls;

public abstract class ItemsPanelTemplateSelector
{
  public abstract ItemsPanelTemplate SelectTemplate(object item, DependencyObject container);
}
