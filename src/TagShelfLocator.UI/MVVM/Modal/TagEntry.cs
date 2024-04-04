namespace TagShelfLocator.UI.MVVM.Modal;

using System.Collections.ObjectModel;

using FEDM;

public class TagEntry
{
  // General
  public int Number { get; private set; } = 0;

  // General Tag Details
  public string TagType { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public int RSSI { get; private set; } = 0;
  public ObservableCollection<Antenna>? Antennas { get; set; }

  // ISO14443-A and ISO15693
  public bool IsISO14443A { get; private set; } = false;
  public bool IsISO15693 { get; private set; } = false;
  public string ManufacturerName { get; private set; } = string.Empty;
  public int Afi { get; private set; } = 0;

  // EPC Class 1 Gen 2
  public bool IsEPCC1G2 { get; private set; } = false;
  public uint ProtocolControl { get; private set; } = 0;
  public string EPCHex { get; private set; } = string.Empty;
  public string TIDHex { get; private set; } = string.Empty;

  public int TagModelNumber { get; private set; } = 0;
  public string TagDesignerName { get; private set; } = string.Empty;


  public override string ToString()
  {
    return $"{Number}: '{SerialNumber}' - ⭐ {ManufacturerName}";
  }

  public static TagEntry FromData(int count, string serialNumber, string trType)
  {
    return new TagEntry
    {
      Number = count,
      SerialNumber = serialNumber,
      TagType = trType,
      Antennas = new(),
    };
  }

  public static TagEntry FromOBIDTagItem(int count, TagItem tagItem)
  {
    var tagEntry = new TagEntry();

    var tagType = TransponderType.toString(tagItem.trType());
    var serialNumber = tagItem.iddToHexString();

    tagEntry.Number = count;
    tagEntry.TagType = tagType;
    tagEntry.SerialNumber = serialNumber;
    tagEntry.Antennas = GetAntennas(tagItem);

    if (tagItem.isIso14443A())
      return CreateISO1443A_TagEntry(tagItem, tagEntry);

    if (tagItem.isIso15693())
      return CreateISO15693_TagEntry(tagItem, tagEntry);

    if (tagItem.isEpcClass1Gen2())
      return CreateEPCClass1Gen2_TagEntry(tagItem, tagEntry);

    return tagEntry;
  }

  private static ObservableCollection<Antenna> GetAntennas(TagItem tagItem)
  {
    var antennas = new ObservableCollection<Antenna>();

    if (tagItem.rssiValues != null)
      foreach (var rssi in tagItem.rssiValues())
        antennas.Add(new Antenna
        {
          AntennaNo = rssi.antennaNumber(),
          RSSI = rssi.rssi(),
        });

    return antennas;
  }

  private static TagEntry CreateISO1443A_TagEntry(TagItem tagItem, TagEntry tagEntry)
  {
    var manufacturerName = tagItem.manufacturerName();

    tagEntry.IsISO14443A = true;
    tagEntry.ManufacturerName = manufacturerName;

    return tagEntry;
  }

  private static TagEntry CreateISO15693_TagEntry(TagItem tagItem, TagEntry tagEntry)
  {
    var afi = tagItem.iso15693_Afi();

    tagEntry.IsISO15693 = true;
    tagEntry.Afi = afi;

    return tagEntry;
  }

  private static TagEntry CreateEPCClass1Gen2_TagEntry(TagItem tagItem, TagEntry tagEntry)
  {
    var pc = tagItem.epcC1G2_Pc();
    var epchex = tagItem.epcC1G2_EpcToHexString();

    var tidhex = string.Empty;
    var tagModelNumber = 0;
    var tagDesignerName = string.Empty;

    if (tagItem.epcC1G2_IsEpcAndTid())
    {
      tidhex = tagItem.epcC1G2_TidToHexString();
      tagModelNumber = tagItem.epcC1G2_TagModelNumber();
      tagDesignerName = tagItem.epcC1G2_MaskDesignerName();
    }

    tagEntry.IsEPCC1G2 = true;
    tagEntry.ProtocolControl = pc;
    tagEntry.EPCHex = epchex;
    tagEntry.TIDHex = tidhex;
    tagEntry.TagModelNumber = tagModelNumber;
    tagEntry.TagDesignerName = tagDesignerName;

    return tagEntry;
  }
}
