namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReaderManager
{
  ReaderDescription SelectedReader { get; }
  ReaderDescription this[uint deviceID] { get; }
  uint[] GetDeviceIDs();
  IReadOnlyList<ReaderDescription> GetReaderDescriptions();
  void SetSelectedReader(uint readerId);
  bool TryGetReaderByDeviceID(uint deviceID, out ReaderDescription reader);
  void AddReaderDescription(uint deviceID, ReaderDescription description);
  void RemoveReaderDescription(uint deviceID);
  Task<bool> ConnectReader(uint deviceID);
  Task<bool> DisconnectReader(uint deviceID);
}
