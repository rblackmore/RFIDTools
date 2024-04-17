namespace TagShelfLocator.UI.MVVM.ViewModels;

public interface IReaderManagementVM : IViewModel
{
  bool IsConnected { get; }
  string? ReaderName { get; }
  uint DeviceID { get; }
  void SelectedReaderChanged(uint deviceId);
}
