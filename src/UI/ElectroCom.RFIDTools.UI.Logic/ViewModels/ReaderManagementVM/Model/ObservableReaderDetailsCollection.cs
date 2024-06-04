namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System.Collections.ObjectModel;

/// <summary>
/// Observable Collection of <see cref="ObservableReaderDetails"/>.
/// </summary>
public class ObservableReaderDetailsCollection : ObservableCollection<ObservableReaderDetails>
{
  protected override void InsertItem(int index, ObservableReaderDetails item)
  {
    if (this.Contains(item))
    {
      return;
    }

    base.InsertItem(index, item);
  }
}
