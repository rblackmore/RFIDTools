namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Input;

public interface ISelectedReaderStatusVM : IViewModel
{
  bool IsConnected { get; }
  string DeviceName { get; }
  uint DeviceID { get; }

  IRelayCommand ConnectReader { get; }
  IRelayCommand DisconnectReader { get; }
}
