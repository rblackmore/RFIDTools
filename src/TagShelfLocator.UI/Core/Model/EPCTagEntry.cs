namespace TagShelfLocator.UI.Core.Model;

using System.Collections.Generic;
using System.Linq;

public class EPCTagEntry
{
  private byte[] idd;
  private readonly string epc;
  private readonly string tid;
  private readonly IReadOnlyList<Antenna> antennas;

  public EPCTagEntry(byte[] idd, string epc, string tid, IReadOnlyList<Antenna> antennas)
  {
    this.idd = idd;
    this.epc = epc;
    this.tid = tid;
    this.antennas = antennas;
  }

  public byte[] IDD => this.idd;
  public string EPC => this.epc;
  public string TID => this.tid;

  public byte[] ProtocolControl()
  {
    return this.idd.Take(2).ToArray();
  }
}
