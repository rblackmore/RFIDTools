namespace ElectroCom.RFIDTools.ReaderServices;

using ElectroCom.RFIDTools.ReaderServices.TagReading.Implementations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
public class TagReaderFactory : ITagReaderFactory
{
  private readonly IServiceProvider services;
  private readonly IReaderManager readerManager;

  public TagReaderFactory(IServiceProvider services, IReaderManager readerManager)
  {
    this.services = services;
    this.readerManager = readerManager;
  }

  public ITagReader Create(TagReaderOptions options)
  {
    var rd = this.readerManager.SelectedReader;
    var logger = this.services.GetRequiredService<ILogger<InventoryTagReader>>();

    return new InventoryTagReader(logger, rd, options);

    //TODO: Create a different TagReader Implementations based on options provided.
  }

  public ITagReader CreateNullTagReader(TagReaderOptions options)
  {
    var logger = this.services.GetRequiredService<ILogger<NullTagReader>>();

    return new NullTagReader(logger, options);
  }
}
