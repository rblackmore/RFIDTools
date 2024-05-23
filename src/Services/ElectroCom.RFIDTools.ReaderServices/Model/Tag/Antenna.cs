namespace ElectroCom.RFIDTools.ReaderServices.Model.Tag;

using System.Collections.Generic;

public class Antenna : ValueObject
{
  public Antenna(int antNo, int rssi = 0)
  {
    this.AntennaNo = antNo;
    this.RSSI = rssi;
  }

  public int AntennaNo { get; private set; }
  public int RSSI { get; private set; }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return this.AntennaNo;
    yield return this.RSSI;
  }
}
