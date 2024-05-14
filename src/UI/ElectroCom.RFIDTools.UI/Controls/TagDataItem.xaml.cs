namespace ElectroCom.RFIDTools.UI.Controls;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for LabelValueUserControl.xaml
/// </summary>
public partial class TagDataItem : UserControl
{
  public TagDataItem()
  {
    InitializeComponent();
  }

  public string Label
  {
    get { return (string)GetValue(LabelProperty); }
    set { SetValue(LabelProperty, value); }
  }
  public static readonly DependencyProperty LabelProperty =
      DependencyProperty.Register("Label", typeof(string), typeof(TagDataItem),
        new PropertyMetadata("Label"));

  public string Value
  {
    get { return (string)GetValue(ValueProperty); }
    set { SetValue(ValueProperty, value); }
  }
  public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register("Value", typeof(string), typeof(TagDataItem), new PropertyMetadata("Value"));
}
