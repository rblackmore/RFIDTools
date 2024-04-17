namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using ElectroCom.RFIDTools.UI.Logic.Modal;

public class DummyInventoryViewModel : ViewModel, IInventoryViewModel
{
  public ObservableTagList TagList
  {
    get
    {
      return new ObservableTagList(
        new[] {
          TagEntry.FromData(1, "EPC Class 1 Gen 2", "1234"),
          TagEntry.FromData(2, "ISO14443-A Mifare DESFire", "ABCD"),
          TagEntry.FromData(3, "EPC Class 1 Gen 2", "4321"),
          TagEntry.FromData(4, "EPC Class 1 Gen 2", "DEF1"),
        });
    }
  }

  public ObservableTagList TagListData => throw new System.NotImplementedException();

  public bool ClearOnStart { get; set; } = true;

  public IRelayCommand ClearTagList => throw new System.NotImplementedException();

  public IAsyncRelayCommand StartInventoryAsync => throw new System.NotImplementedException();

  public IAsyncRelayCommand StopInventoryAsync => throw new System.NotImplementedException();

  public IRelayCommand OpenSettings => throw new System.NotImplementedException();

  public bool IsReaderConnected => true;

  public bool IsReaderDisconnected => false;

  public IRelayCommand AddTagEntry => throw new System.NotImplementedException();

}
