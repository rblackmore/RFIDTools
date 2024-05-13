namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Input;

public class DummyInventoryViewModel : ViewModel, IInventoryViewModel
{
  public ObservableTagEntryCollection TagList
  {
    get
    {
      return null!;
    }
  }

  public ObservableTagEntryCollection TagListData => throw new System.NotImplementedException();

  public bool ClearOnStart { get; set; } = true;

  public IRelayCommand ClearTagList => throw new System.NotImplementedException();

  public IAsyncRelayCommand StartInventoryAsync => throw new System.NotImplementedException();

  public IAsyncRelayCommand StopInventoryAsync => throw new System.NotImplementedException();

  public IRelayCommand OpenSettings => throw new System.NotImplementedException();

  public bool IsReaderConnected => true;

  public bool IsReaderDisconnected => false;

  public IRelayCommand AddTagEntry => throw new System.NotImplementedException();

  public bool Antenna1 { get => false; set { } }
  public bool Antenna2 { get => false; set { } }
  public bool Antenna3 { get => false; set { } }
  public bool Antenna4 { get => false; set { } }

  public string PollingFeedback => "17 Tags Read in Inventory";
}
