namespace TagShelfLocator.UI.MVVM.Modal;

using CommunityToolkit.Mvvm.ComponentModel;

public class Antenna : ObservableObject
{
  private int antennaNo;
  private int rssi;

  public int AntennaNo
  {
    get => antennaNo;
    set => SetProperty(ref antennaNo, value);
  }
  public int RSSI
  {
    get => rssi;
    set => SetProperty(ref rssi, value);
  }
}
