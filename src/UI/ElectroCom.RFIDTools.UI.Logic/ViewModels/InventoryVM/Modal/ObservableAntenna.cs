namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

using ElectroCom.RFIDTools.ReaderServices.Model.Tag;

public class ObservableAntenna : ObservableObject, IEquatable<ObservableAntenna>
{
  private readonly Antenna antenna;

  private int antennaNo;
  private int rssi;

  public ObservableAntenna(Antenna antenna)
  {
    this.antenna = antenna;
    this.AntennaNo = this.antenna.AntennaNo;
    this.RSSI = this.antenna.RSSI;
  }

  public int AntennaNo
  {
    get => this.antennaNo;
    set => SetProperty(ref this.antennaNo, value);
  }

  public int RSSI
  {
    get => this.rssi;
    set => SetProperty(ref this.rssi, value);
  }

  public bool Equals(ObservableAntenna? other)
  {
    if (other is null)
      return false;

    if (ReferenceEquals(this, other))
      return true;

    return this.antenna == other.antenna;
  }
}
