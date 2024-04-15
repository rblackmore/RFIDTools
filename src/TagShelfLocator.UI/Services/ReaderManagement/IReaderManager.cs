namespace TagShelfLocator.UI.Services.ReaderManagement;

using FEDM;

public interface IReaderManager
{
  ReaderModule SelectedReader { get; }
  ReaderModule this[uint deviceID] { get; }
  uint[] GetReaderIDs();
  bool SetSelectedReader(uint readerId);
  bool TryGetReaderByDeviceID(uint deviceID, out ReaderModule reader);
}
