namespace TagShelfLocator.UI.MVVM.ViewModels;

public interface IReaderConnectionStatusViewModel : IViewModel
{
  bool IsConnected { get; }
  string ReaderName { get; }
  uint DeviceID { get; }
}
