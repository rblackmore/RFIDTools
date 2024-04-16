namespace TagShelfLocator.UI.Services.ReaderManagement;

using System.Collections.Generic;

using FEDM;

using TagShelfLocator.UI.Services.ReaderManagement.Model;

public interface IReaderManager
{
  ReaderDescription SelectedReader { get; }
  ReaderDescription this[uint deviceID] { get; }
  uint[] GetReaderIDs();
  IReadOnlyList<ReaderDescription> GetReaderDescriptions();
  bool SetSelectedReader(uint readerId);
  bool TryGetReaderByDeviceID(uint deviceID, out ReaderDescription reader);
}
