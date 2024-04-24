namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

public interface IReaderManagementVM : IViewModel
{
  bool IsConnected { get; }
  string DeviceName { get; }
  uint DeviceID { get; }
}
