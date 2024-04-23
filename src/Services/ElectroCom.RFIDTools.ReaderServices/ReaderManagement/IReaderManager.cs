namespace ElectroCom.RFIDTools.ReaderServices;

public interface IReaderManager
{
  public ReaderDefinition SelectedReader { get; }
  IReadOnlyCollection<ReaderDefinition> GetReaderDefinitions();
  public void RegisterReader(ReaderDefinition readerDefinition);
  bool UnregisterReader(ReaderDefinition rdToRemove);
  bool UnregisterReader(uint deviceId);
  bool UnregisterSelectedReader();
  bool SelectReader(int idx);
  bool SelectReader(uint deviceId);
  bool SelectReader(ReaderDefinition? rd);
}
