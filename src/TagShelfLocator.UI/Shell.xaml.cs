namespace TagShelfLocator.UI;

using System.Windows;
using System.Windows.Input;

/// <summary>
/// Interaction logic for Shell.xaml
/// </summary>
public partial class Shell : Window
{
  public Shell()
  {
    InitializeComponent();
  }

  private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
  {
    if (e.LeftButton == MouseButtonState.Pressed)
    {
      DragMove();
    }
  }
}
