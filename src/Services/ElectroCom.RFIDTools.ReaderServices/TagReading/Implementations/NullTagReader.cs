namespace ElectroCom.RFIDTools.ReaderServices.TagReading.Implementations;

using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using ElectroCom.RFIDTools.ReaderServices.Model;

using Microsoft.Extensions.Logging;

/// <summary>
/// This class is incomplete, it's meant to simulate tag reading when a Reader is not connected.
/// </summary>
public class NullTagReader : ITagReader
{
  private readonly ILogger<NullTagReader> logger;
  private readonly TagReaderOptions options;

  private CancellationTokenSource cts;
  private Task? readingTask;


  public NullTagReader(ILogger<NullTagReader> logger, TagReaderOptions options)
  {
    ArgumentNullException.ThrowIfNull(options, nameof(options));

    this.options = options;
    this.logger = logger;
  }

  public bool IsRunning => throw new NotImplementedException();

  public bool CanStart => true;

  public async Task<TagReaderChannels> StartReadingAsync(CancellationToken token = default)
  {
    if (this.IsRunning)
      throw new Exception("Reading Task Already Running.");

    var (dataChannel, statusChannel) = TagReaderChannels.CreateChannels();

    this.cts = CancellationTokenSource.CreateLinkedTokenSource(token);

    _ = Task.Run(
      async () => await ExecuteAsync(dataChannel.Writer, statusChannel.Writer, cts.Token),
      token);

    return new TagReaderChannels(dataChannel.Reader, statusChannel.Reader);
  }

  public async Task StopReadingAsync(CancellationToken token = default)
  {
    if (this.cts is not null)
      await this.cts.CancelAsync();
  }

  private async Task ExecuteAsync(
    ChannelWriter<TagReaderDataReport> dataWriter,
    ChannelWriter<TagReaderProcessStatusUpdate> statusWriter,
    CancellationToken token)
  {
    try
    {
      var delay = Random.Shared.Next(3, 11);

      await Task.Delay(delay * 1000);

      var count = Random.Shared.Next(1, 4);

      var tagList = new List<TagEntry>();

      for (int i = 0; i < count; i++)
      {
        var serial = Random.Shared.Next(10000, 100000);

        tagList.Add(new TagEntry("NullTagType", $"{serial}"));
      }

      var dataReport =
        new TagReaderDataReport(tagList, $"{tagList.Count} null tag reads");

      await dataWriter.WriteAsync(dataReport);
    }
    catch (Exception ex)
    {
      var statusReport =
        new TagReaderProcessStatusUpdate(ex.Message, TagReaderProcessState.Faulted);

      await statusWriter.WriteAsync(statusReport);
    }
  }
}
