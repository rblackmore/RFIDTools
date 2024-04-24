namespace ElectroCom.RFIDTools.ReaderServices;

public interface IReaderManager
{
  ReaderDefinition? SelectedReader { get; }
  IReadOnlyCollection<ReaderDefinition> GetReaderDefinitions();
  void RegisterReader(ReaderDefinition readerDefinition);
  void UnregisterReader(ReaderDefinition rdToRemove);
  void SelectReaderByIndex(int idx);
  void SelectReader(ReaderDefinition readerDefinition);
}
