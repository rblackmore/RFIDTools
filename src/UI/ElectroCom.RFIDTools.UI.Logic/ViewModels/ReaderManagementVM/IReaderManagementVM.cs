namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

public interface IReaderManagementVM : IViewModel
{
  bool IsConnected { get; }
  string? ReaderName { get; }
  uint DeviceID { get; }
}
