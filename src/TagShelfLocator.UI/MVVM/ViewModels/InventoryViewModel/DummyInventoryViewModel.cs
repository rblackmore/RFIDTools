namespace TagShelfLocator.UI.MVVM.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.MVVM.Modal;

public class DummyInventoryViewModel : ViewModel, IInventoryViewModel
{
  public ObservableCollection<TagEntry> TagList
  {
    get
    {
      return new ObservableCollection<TagEntry>(
        new[] {
          TagEntry.FromData(1, "EPC Class 1 Gen 2", "1234"),
          TagEntry.FromData(2, "ISO14443-A Mifare DESFire", "ABCD"),
          TagEntry.FromData(3, "EPC Class 1 Gen 2", "4321"),
          TagEntry.FromData(4, "EPC Class 1 Gen 2", "DEF1"),
        });
    }
  }
  
  public TagListViewModel TagListData => throw new System.NotImplementedException();

  public bool ClearOnStart { get; set; } = true;

  public IRelayCommand ClearTagList => throw new System.NotImplementedException();

  public IAsyncRelayCommand StartInventoryAsync => throw new System.NotImplementedException();

  public IAsyncRelayCommand StopInventoryAsync => throw new System.NotImplementedException();

  public IRelayCommand OpenSettings => throw new System.NotImplementedException();

  public bool IsReaderConnected => true;

  public bool IsReaderDisconnected => false;

  public IRelayCommand AddTagEntry => throw new System.NotImplementedException();

}
