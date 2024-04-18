namespace ElectroCom.RFIDTools.UI.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// Interaction logic for WindowTitleBar.xaml
/// </summary>
public partial class WindowTitleBar : UserControl
{
  public WindowTitleBar()
  {
    InitializeComponent();
  }

  public event EventHandler? CloseButton;
  public event EventHandler? MaximizeButton;
  public event EventHandler? MinimizeButton;

  public event EventHandler<MouseButtonEventArgs>? WindowMouseDown;

  private void Border_MouseDown(object sender, MouseButtonEventArgs e)
  {
    this.WindowMouseDown?.Invoke(this, e);
  }

  private void WindowMinimize_Click(object sender, RoutedEventArgs e)
  {
    this.MinimizeButton?.Invoke(this, e);
  }

  private void WindowMaximize_Click(object sender, RoutedEventArgs e)
  {
    this.MaximizeButton?.Invoke(this, e);
  }

  private void WindowClose_Click(object sender, RoutedEventArgs e)
  {
    this.CloseButton?.Invoke(this, e);
  }
}
