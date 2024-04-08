namespace TagShelfLocator.UI.MVVM.ViewModels;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

using TagShelfLocator.UI.MVVM.Modal;

public class ObservableTagList : ObservableCollection<TagEntry>
{
  public ObservableTagList()
    : base() { }

  public ObservableTagList(IEnumerable<TagEntry> items)
    : base(items) { }

  public ObservableTagList(List<TagEntry> items)
    : base(items) { }

  private bool incrementReadCountOnAdd = true;

  public bool IncrementReadCountOnAdd
  {
    get => incrementReadCountOnAdd;
    set
    {
      this.incrementReadCountOnAdd = value;
      this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(IncrementReadCountOnAdd)));
    }
  }

  protected override void InsertItem(int index, TagEntry item)
  {
    if (!this.IncrementReadCountOnAdd || !this.Contains(item))
    {
      base.InsertItem(index, item);
      return;
    }

    this.First(entry => entry.SerialNumber == item.SerialNumber).IncrementRead();
  }
}
