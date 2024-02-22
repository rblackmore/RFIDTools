namespace TagShelfLocator.UI.Core.Model;
public class Antenna
{
  private int antennaNumber;
  private int rssi;

  public Antenna(int antennaNumber, int rssi)
  {
    this.antennaNumber = antennaNumber;
    this.rssi = rssi;
  }

  public int AntennaNumber => antennaNumber;
  public int RSSI => rssi;
}
