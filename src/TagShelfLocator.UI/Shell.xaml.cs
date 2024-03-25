namespace TagShelfLocator.UI;

using System.Windows;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
  }

  private void WindowMaximize_Click(object sender, RoutedEventArgs e)
  {
  }

  private void WindowMinimize_Click(object sender, RoutedEventArgs e)
  {
  }

  private void WindowTitleBar_CloseButton(object sender, System.EventArgs e)
  {
    var appLifetime = App.Current.Services.GetRequiredService<IHostApplicationLifetime>();

    appLifetime.StopApplication();

    App.Current.Shutdown();
  }

  private void WindowTitleBar_MaximizeButton(object sender, System.EventArgs e)
  {
    if (this.WindowState is not WindowState.Maximized)
      this.WindowState = WindowState.Maximized;
    else
      this.WindowState = WindowState.Normal;

  }

  private void WindowTitleBar_MinimizeButton(object sender, System.EventArgs e)
  {
    this.WindowState = WindowState.Minimized;

  }

  private void WindowTitleBar_WindowMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
  {

    if (e.LeftButton == MouseButtonState.Pressed)
    {
      DragMove();
    }
  }
}
