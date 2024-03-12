namespace TagShelfLocator.UI.Core.Model;

using System;
using System.Linq;
using System.Threading.Tasks;

using FEDM;

public class TagEntry
{
  public int Number { get; set; }
  public string TagType { get; set; }
  public string SerialNumber { get; set; }
  public int RSSI { get; set; }

  public TagEntry(int number, string tagType, string serialNumber, int rssi)
  {
    this.Number = number;
    this.TagType = tagType;
    this.SerialNumber = serialNumber;
    this.RSSI = rssi;
  }

  public override string ToString()
  {
    return $"No. {Number} - Type: {TagType} - Serial: {SerialNumber} - RSSI: {RSSI}";
  }

  public static TagEntry FromOBIDTagItem(int count, TagItem tagItem)
  {
    var tagType = TransponderType.toString(tagItem.trType());
    var serialNumber = tagItem.iddToHexString();
    var rssi = 0;

    var rssiValues = tagItem.rssiValues();

    if (rssiValues is not null)
    {
      foreach (var value in rssiValues)
      {
        rssi = value.rssi();
      }
    }

    return new TagEntry(count, tagType, serialNumber, rssi);
  }
}
