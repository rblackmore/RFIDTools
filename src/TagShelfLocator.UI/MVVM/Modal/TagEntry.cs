namespace TagShelfLocator.UI.MVVM.Modal;

using System;

using FEDM;

public class TagEntry
{
  // General
  public int Number { get; init; } = 0;

  // General Tag Details
  public string TagType { get; init; } = string.Empty;
  public string SerialNumber { get; init; } = string.Empty;
  public int RSSI { get; init; } = 0;

  // ISO14443-A and ISO15693
  public bool IsISO14443A { get; init; } = false;
  public bool IsISO15693 { get; init; } = false;
  public string ManufacturerName { get; init; } = string.Empty;
  public int Afi { get; init; } = 0;

  // EPC Class 1 Gen 2
  public bool IsEPCC1G2 { get; init; } = false;
  public uint ProtocolControl { get; init; } = 0;
  public string EPCHex { get; init; } = string.Empty;
  public string TIDHex { get; init; } = string.Empty;

  public int TagModelNumber { get; init; } = 0;
  public string TagDesignerName { get; init; } = string.Empty;


  public override string ToString()
  {
    return $"{Number}: '{SerialNumber}' - ⭐ {ManufacturerName}";
  }

  public static TagEntry FromOBIDTagItem(int count, TagItem tagItem)
  {

    if (tagItem.isIso14443A())
      return CreateISO1443A_TagEntry(count, tagItem);

    if (tagItem.isIso15693())
      return CreateISO15693_TagEntry(count, tagItem);

    if (tagItem.isEpcClass1Gen2())
      return CreateEPCClass1Gen2_TagEntry(count, tagItem);

    // If tag type is not discovered above, we get a default entry.
    var tagType = TransponderType.toString(tagItem.trType());
    var serialNumber = tagItem.iddToHexString();

    return new TagEntry
    {
      Number = count,
      TagType = tagType,
      SerialNumber = serialNumber,
    };
  }

  private static TagEntry CreateISO1443A_TagEntry(int count, TagItem tagItem)
  {
    var tagType = TransponderType.toString(tagItem.trType());
    var serialNumber = tagItem.iddToHexString();
    var manufacturerName = tagItem.manufacturerName();

    return new TagEntry
    {
      IsISO14443A = true,
      Number = count,
      TagType = tagType,
      SerialNumber = serialNumber,
      ManufacturerName = manufacturerName,
    };
  }

  private static TagEntry CreateISO15693_TagEntry(int count, TagItem tagItem)
  {
    var tagType = TransponderType.toString(tagItem.trType());
    var serialNumber = tagItem.iddToHexString();
    var afi = tagItem.iso15693_Afi();

    return new TagEntry
    {
      IsISO15693 = true,
      Number = count,
      TagType = tagType,
      SerialNumber = serialNumber,
      Afi = afi,
    };
  }

  private static TagEntry CreateEPCClass1Gen2_TagEntry(int count, TagItem tagItem)
  {
    var tagType = TransponderType.toString(tagItem.trType());
    var serialNumber = tagItem.iddToHexString();

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

    return new TagEntry
    {
      IsEPCC1G2 = true,
      Number = count,
      TagType = tagType,
      SerialNumber = serialNumber,
      ProtocolControl = pc,
      EPCHex = epchex,
      TIDHex = tidhex,
      TagModelNumber = tagModelNumber,
      TagDesignerName = tagDesignerName,
    };
  }
}
