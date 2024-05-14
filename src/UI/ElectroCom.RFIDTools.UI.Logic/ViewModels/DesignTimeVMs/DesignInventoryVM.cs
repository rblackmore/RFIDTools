namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Input;

using ElectroCom.RFIDTools.ReaderServices.Model;

public class DesignInventoryVM : ViewModel, IInventoryViewModel
{

  public DesignInventoryVM()
  {
    this.TagList = [];
    this.TagList.Add(new ObservableTagEntry(new TagEntry("EPCC1G2", "12345")));
    this.TagList.Add(new ObservableTagEntry(new TagEntry("EPCC1G2", "12345")));
    this.TagList.Add(new ObservableTagEntry(new TagEntry("EPCC1G2", "12345")));
    this.TagList.Add(new ObservableTagEntry(new TagEntry("EPCC1G2", "12345")));
    this.TagList.Add(new ObservableTagEntry(new TagEntry("EPCC1G2", "12345")));
  }

  public bool IsReaderConnected => true;

  public bool IsReaderDisconnected => false;

  public ObservableTagEntryCollection TagList { get; }

  private string pollingFeedback = "17 Tag Reads";
  private bool clearOnStart = true;
  private bool ant1 = true;
  private bool ant2 = true;
  private bool ant3 = true;
  private bool ant4 = true;

  public string PollingFeedback { get => pollingFeedback; set => SetProperty(ref pollingFeedback, value); }
  public bool ClearOnStart { get => clearOnStart; set => SetProperty(ref clearOnStart, value); }
  public bool Antenna1 { get => ant1; set => SetProperty(ref ant1, value); }
  public bool Antenna2 { get => ant2; set => SetProperty(ref ant2, value); }
  public bool Antenna3 { get => ant3; set => SetProperty(ref ant3, value); }
  public bool Antenna4 { get => ant4; set => SetProperty(ref ant4, value); }

  public IRelayCommand ClearTagList => throw new NotImplementedException();

  public IAsyncRelayCommand StartInventoryAsync => throw new NotImplementedException();

  public IAsyncRelayCommand StopInventoryAsync => throw new NotImplementedException();

  public IRelayCommand OpenSettings => throw new NotImplementedException();
}
