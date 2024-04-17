namespace ElectroCom.RFIDTools.UI.Logic.Modal;

using System;
using System.Collections.ObjectModel;
using System.Timers;

using CommunityToolkit.Mvvm.ComponentModel;

using FEDM;

public class TagEntry : ObservableObject
{
  private int number = 0;
  private int readCount = 0;
  private TimeOnly lastRead = TimeOnly.FromDateTime(System.DateTime.Now);
  private int secondsSinceLastRead = 0;
  private string tagType = string.Empty;
  private string serialNumber = string.Empty;

  public TagEntry()
  {
    var timer = new Timer(1000);
    timer.Elapsed += UpdateTimerElapsed;
    timer.Start();
  }

  private void UpdateTimerElapsed(object? sender, ElapsedEventArgs e)
  {
    var now = System.DateTime.Now;
    var last = new System.DateTime(this.LastRead.Ticks);

    this.SecondsSinceLastRead = (now - last).Seconds;
  }

  // General
  public int Number
  {
    get => number;
    private set => SetProperty(ref number, value);
  }
  public int ReadCount
  {
    get => this.readCount;
    private set => SetProperty(ref readCount, value);
  }

  public TimeOnly LastRead
  {
    get => this.lastRead;
    private set => SetProperty(ref lastRead, value);
  }

  public int SecondsSinceLastRead
  {
    get => this.secondsSinceLastRead;
    set => SetProperty(ref secondsSinceLastRead, value);
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

  public void IncrementRead()
  {
    this.ReadCount++;
    this.LastRead = TimeOnly.FromDateTime(System.DateTime.Now);
  }

  public override string ToString()
  {
    return $"{Number}: '{SerialNumber}' - {TagType}";
  }

  public override bool Equals(object? obj)
  {
    if (obj is not TagEntry other)
      return false;

    if (ReferenceEquals(this, other))
      return true;

    if (other is null)
      return false;

    return this.SerialNumber == other.SerialNumber;
  }

  public static bool operator ==(TagEntry left, TagEntry right)
  {
    if (left is null && right is null)
      return true;

    if (left is null || right is null)
      return false;

    if (left.SerialNumber is null || right.SerialNumber is null)
      return false;

    return left.Equals(right);
  }

  public static bool operator !=(TagEntry left, TagEntry right)
  {
    return !(left == right);
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
