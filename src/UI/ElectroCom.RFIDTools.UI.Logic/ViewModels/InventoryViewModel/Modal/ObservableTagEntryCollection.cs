namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

public class ObservableTagEntryCollection : ObservableCollection<ObservableTagEntry>
{
  public ObservableTagEntryCollection()
    : base() { }

  public ObservableTagEntryCollection(IEnumerable<ObservableTagEntry> items)
    : base(items) { }

  public ObservableTagEntryCollection(List<ObservableTagEntry> items)
    : base(items) { }

  private bool incrementReadCountOnAdd = true;

  public bool IncrementReadCountOnAdd
  {
    get => incrementReadCountOnAdd;
    set
    {
      incrementReadCountOnAdd = value;
      OnPropertyChanged(new PropertyChangedEventArgs(nameof(IncrementReadCountOnAdd)));
    }
  }

  protected override void InsertItem(int index, ObservableTagEntry item)
  {
    if (!IncrementReadCountOnAdd || !Contains(item))
    {
      base.InsertItem(index, item);
      return;
    }

    this.First(entry => entry.SerialNumber == item.SerialNumber).IncrementReadCount();
  }
}
