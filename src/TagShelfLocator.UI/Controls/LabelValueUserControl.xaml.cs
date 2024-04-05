namespace TagShelfLocator.UI.Controls;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for LabelValueUserControl.xaml
/// </summary>
public partial class LabelValueUserControl : UserControl
{
  public LabelValueUserControl()
  {
    InitializeComponent();
  }

  public string Label
  {
    get { return (string)GetValue(LabelProperty); }
    set { SetValue(LabelProperty, value); }
  }
  public static readonly DependencyProperty LabelProperty =
      DependencyProperty.Register("Label", typeof(string), typeof(LabelValueUserControl),
        new PropertyMetadata("Label"));

  public string Value
  {
    get { return (string)GetValue(ValueProperty); }
    set { SetValue(ValueProperty, value); }
  }
  public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register("Value", typeof(string), typeof(LabelValueUserControl), new PropertyMetadata("Value"));
}
