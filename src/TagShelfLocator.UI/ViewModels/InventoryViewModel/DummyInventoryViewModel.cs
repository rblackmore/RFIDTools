namespace TagShelfLocator.UI.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Core.Model;

public class DummyInventoryViewModel : ViewModel, IInventoryViewModel
{
  public ObservableCollection<TagEntry> TagList
  {
    get
    {
      return new ObservableCollection<TagEntry>(
        new[] {
          new TagEntry(1, "EPC Class 1 Gen 2", "1234", -13),
          new TagEntry(2, "ISO14443-A Mifare DESFire", "ABCD", -16),
          new TagEntry(3, "EPC Class 1 Gen 2", "4321", -11),
          new TagEntry(4, "EPC Class 1 Gen 2", "DEF1", -9),
        });
    }
  }
  public bool ClearOnStart { get; set; } = true;

  public IRelayCommand ClearTagList => throw new System.NotImplementedException();

  public IAsyncRelayCommand StartInventoryAsync => throw new System.NotImplementedException();

  public IAsyncRelayCommand StopInventoryAsync => throw new System.NotImplementedException();

  public IRelayCommand OpenSettings => throw new System.NotImplementedException();

  public bool IsReaderConnected => true;

  public bool IsReaderDisconnected => false;

  public IRelayCommand AddTagEntry => throw new System.NotImplementedException();

}
