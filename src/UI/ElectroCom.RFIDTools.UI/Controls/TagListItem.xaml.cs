namespace ElectroCom.RFIDTools.UI.Controls;

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

using ElectroCom.RFIDTools.UI.Logic.ViewModels;

/// <summary>
/// Interaction logic for TagListItem.xaml
/// </summary>
public partial class TagListItem : UserControl
{
  private const double SMALL_FONT_SCALE = 0.8d;

  static TagListItem()
  {
    FontSizeProperty.OverrideMetadata(typeof(TagListItem), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnFontSizeChanged)));

  }

  public TagListItem()
  {
    InitializeComponent();
  }


  public bool ReadRecently
  {
    get { return (bool)GetValue(ReadRecentlyProperty); }
    set { SetValue(ReadRecentlyProperty, value); }
  }

  // Using a DependencyProperty as the backing store for ReadRecently.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty ReadRecentlyProperty =
      DependencyProperty.Register("ReadRecently", typeof(bool), typeof(TagListItem), new PropertyMetadata(false));



  #region DependencyProperties
  public static readonly DependencyProperty SerialNumberProperty =
      DependencyProperty.Register(
        "SerialNumber",
        typeof(string),
        typeof(TagListItem),
        new PropertyMetadata("12345678"));

  public static readonly DependencyProperty TagTypeProperty =
    DependencyProperty.Register("TagType", typeof(string), typeof(TagListItem), new PropertyMetadata(String.Empty));

  public static readonly DependencyProperty ReadCountProperty =
      DependencyProperty.Register(
        "ReadCount",
        typeof(int),
        typeof(TagListItem),
        new PropertyMetadata(0));

  public static readonly DependencyProperty AntennasProperty =
    DependencyProperty.Register(
      "Antennas",
      typeof(ObservableCollection<ObservableAntenna>),
      typeof(TagListItem),
      new PropertyMetadata(new ObservableCollection<ObservableAntenna>()));

  public static readonly DependencyProperty SmallFontSizeProperty =
    DependencyProperty.Register(
      "SmallFontSize",
      typeof(double),
      typeof(TagListItem),
      new PropertyMetadata((double)FontSizeProperty.DefaultMetadata.DefaultValue * SMALL_FONT_SCALE));

  #endregion

  #region Properties
  public string SerialNumber
  {
    get { return (string)GetValue(SerialNumberProperty); }
    set { SetValue(SerialNumberProperty, value); }
  }
  public string TagType
  {
    get { return (string)GetValue(TagTypeProperty); }
    set { SetValue(TagTypeProperty, value); }
  }
  public int ReadCount
  {
    get { return (int)GetValue(ReadCountProperty); }
    set { SetValue(ReadCountProperty, value); }
  }
  public ObservableCollection<ObservableAntenna> Antennas
  {
    get { return (ObservableCollection<ObservableAntenna>)GetValue(AntennasProperty); }
    set { SetValue(AntennasProperty, value); }
  }

  public double SmallFontSize
  {
    get { return (double)GetValue(SmallFontSizeProperty); }
    set { SetValue(SmallFontSizeProperty, value); }
  }
  #endregion

  private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is not TagListItem tagListItem)
      return;

    tagListItem.SmallFontSize = (double)e.NewValue * SMALL_FONT_SCALE;
  }
}
