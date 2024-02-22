namespace TagShelfLocator.UI.ViewModels;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Core.Model;

public class DummyInventoryViewModel : ViewModel, IInventoryViewModel
{
  public ObservableCollection<EPCTagEntry> TagList
  {
    get
    {
      return new ObservableCollection<EPCTagEntry>(
        new[] {
        new EPCTagEntry(new byte[] {0x02, 0x03, 0x04}, "020304", "E2008113", new List<Antenna>().AsReadOnly()),
        new EPCTagEntry(new byte[] {0x02, 0x03, 0x04}, "020304", "E2008113", new List<Antenna>().AsReadOnly()),
        new EPCTagEntry(new byte[] {0x02, 0x03, 0x04}, "020304", "E2008113", new List<Antenna>().AsReadOnly()),
        });
    }
  }

  public IAsyncRelayCommand StartInventoryAsync => throw new System.NotImplementedException();

  public IAsyncRelayCommand StopInventoryAsync => throw new System.NotImplementedException();

  public IRelayCommand OpenSettings => throw new System.NotImplementedException();

  public bool IsReaderConnected => true;

  public bool IsReaderDisconnected => false;

}
