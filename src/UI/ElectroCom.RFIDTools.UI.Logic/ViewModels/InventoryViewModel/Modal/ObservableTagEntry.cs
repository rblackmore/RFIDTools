namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

using ElectroCom.RFIDTools.ReaderServices.Model;

public class ObservableTagEntry : ObservableObject, IEquatable<ObservableTagEntry>
{
  private TagEntry tagEntry;

  internal ObservableTagEntry(TagEntry tagEntry)
  {
    this.tagEntry = tagEntry;
  }
  private int readCount;

  public int ReadCount
  {
    get => readCount;
    set => SetProperty(ref readCount, value);
  }

  public string TagType => tagEntry.TagType;
  public string SerialNumber => tagEntry.SerialNumber;

  public void IncrementReadCount()
  {
    ReadCount++;
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
