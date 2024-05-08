namespace ElectroCom.RFIDTools.ReaderServices.TagReading;

using ElectroCom.RFIDTools.ReaderServices.TagReaders;
using ElectroCom.RFIDTools.ReaderServices.TagReaders.InventoryMode;

public enum TagReaderMode
{
  HostMode,
  BufferedReadMode,
  NotificationMode,
  ScanMode,
}

/// <summary>
/// Create a <see cref="ITagReader"/> Intance appropriate for the Current reader Mode Settings.
/// It will inject the Currently Selected Reader from <see cref="IReaderManager"/>
/// </summary>
public class TagReaderFactory
{
  private IReaderManager readerManager;

  public TagReaderFactory(IReaderManager readerManager)
  {
    this.readerManager = readerManager;
  }

  public ITagReader Create(TagReaderOptions options)
  {
    var rd = this.readerManager.SelectedReader;

    return new InventoryTagReader(rd, options);

  }
}
