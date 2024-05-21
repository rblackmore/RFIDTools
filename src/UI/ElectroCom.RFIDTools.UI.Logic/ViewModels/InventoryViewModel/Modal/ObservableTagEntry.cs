namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System.Collections.ObjectModel;
using System.Timers;

using CommunityToolkit.Mvvm.ComponentModel;

using ElectroCom.RFIDTools.ReaderServices.Model;

public class ObservableTagEntry : ObservableObject, IEquatable<ObservableTagEntry>
{
  private TagEntry tagEntry;

  Timer timer;

  internal ObservableTagEntry(TagEntry tagEntry)
  {
    this.tagEntry = tagEntry;
    this.Antennas = new();

    foreach (var ant in tagEntry.Antennas)
    {
      this.Antennas.Add(new ObservableAntenna(ant));
    }
    this.SeenRecently = true;
    this.timer = new Timer(TimeSpan.FromSeconds(1));
    this.timer.Elapsed += NoLongerReadRecently;
    this.timer.AutoReset = false;
    this.timer.Start();
  }

  private void NoLongerReadRecently(object? sender, ElapsedEventArgs e)
  {
    this.SeenRecently = false;

  }

  private int readCount;

  public int ReadCount
  {
    get => readCount;
    set => SetProperty(ref readCount, value);
  }

  public string TagType => tagEntry.TagType;
  public string SerialNumber => tagEntry.SerialNumber;

  private bool seenRecently;

  public bool SeenRecently
  {
    get => this.seenRecently;
    set => SetProperty(ref seenRecently, value);
  }

  public ObservableCollection<ObservableAntenna> Antennas { get; private set; }

  public void IncrementReadCount()
  {
    this.timer.Interval = 1000;
    this.SeenRecently = true;
    ReadCount++;
  }

  public void UpdateAntenna(ObservableAntenna antenna)
  {
    if (!this.Antennas.Contains(antenna))
    {
      this.Antennas.Add(antenna);
      return;
    }

    var existingAntenna = this.Antennas.First(a => a.AntennaNo == antenna.AntennaNo);
    existingAntenna.RSSI = antenna.RSSI;

  }

  public bool Equals(ObservableTagEntry? other)
  {
    if (other is null)
      return false;

    if (ReferenceEquals(this, other))
      return true;

    if (other.SerialNumber == this.SerialNumber)
      return true;

    return false;
  }
}
