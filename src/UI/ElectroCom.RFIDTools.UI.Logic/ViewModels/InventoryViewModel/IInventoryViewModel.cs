namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Input;

public interface IInventoryViewModel : IViewModel
{
  bool IsReaderConnected { get; }
  bool IsReaderDisconnected { get; }
  ObservableTagEntryCollection TagList { get; }
  bool ClearOnStart { get; set; }
  IRelayCommand ClearTagList { get; }
  IAsyncRelayCommand StartInventoryAsync { get; }
  IAsyncRelayCommand StopInventoryAsync { get; }
  IRelayCommand OpenSettings { get; }
}
