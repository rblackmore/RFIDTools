namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

using ElectroCom.RFIDTools.ReaderServices.Model;

public class ObservableTagEntry : ObservableObject
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
}
