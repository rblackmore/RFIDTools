namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Input;

public interface IInventoryViewModel : IViewModel
{
  bool IsReaderConnected { get; }
  bool IsReaderDisconnected { get; }
  ObservableTagEntryCollection TagList { get; }
  string PollingFeedback { get; }
  bool ClearOnStart { get; set; }
  bool Antenna1 { get; set; }
  bool Antenna2 { get; set; }
  bool Antenna3 { get; set; }
  bool Antenna4 { get; set; }
  IRelayCommand ClearTagList { get; }
  IAsyncRelayCommand StartInventoryAsync { get; }
  IAsyncRelayCommand StopInventoryAsync { get; }
  IRelayCommand OpenSettings { get; }
}
