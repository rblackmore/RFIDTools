namespace ElectroCom.RFIDTools.ReaderServices.Model.Tag;

using System.Collections.Generic;

public class Antenna : ValueObject
{
  public int AntennaNo { get; set; }
  public int RSSI { get; set; }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return this.AntennaNo;
    yield return this.RSSI;
  }
}
