namespace TagShelfLocator.UI.MVVM.Modal;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using FEDM;

public class TagEntry : ObservableObject
{
  private int number = 0;
  private string tagType = string.Empty;
  private string serialNumber = string.Empty;

  // General
  public int Number
  {
    get => number;
    private set => SetProperty(ref number, value);
  }
  // General Tag Details
  public string TagType
  {
    get => tagType;
    private set => SetProperty(ref tagType, value);
  }
  public string SerialNumber
  {
    get => serialNumber;
    private set => SetProperty(ref serialNumber, value);
  }

  public ObservableCollection<Antenna>? Antennas { get; set; }

  public override string ToString()
  {
    return $"{Number}: '{SerialNumber}' - {TagType}";
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
    TagEntry tagEntry = null!;

    if (tagItem.isIso14443A())
      tagEntry = ISO14443A_TagEntry.CreateFrom(tagItem);

    if (tagItem.isIso15693())
      tagEntry = ISO15693_TagEntry.CreateFrom(tagItem);

    if (tagItem.isEpcClass1Gen2())
      tagEntry = EPCClass1Gen2_TagEntry.CreateFrom(tagItem);

    if (tagEntry is null)
      tagEntry = new TagEntry();

    var tagType = TransponderType.toString(tagItem.trType());
    var serialNumber = tagItem.iddToHexString();

    tagEntry.Number = count;
    tagEntry.TagType = tagType;
    tagEntry.SerialNumber = serialNumber;
    tagEntry.Antennas = GetAntennas(tagItem);

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
}
