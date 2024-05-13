namespace ElectroCom.RFIDTools.ReaderServices;

using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using ElectroCom.RFIDTools.ReaderServices.Model;

using FEDM;

using Microsoft.Extensions.Logging;

using static FEDM.Hm;

public class InventoryTagReader : ITagReader
{
  private readonly ILogger<InventoryTagReader> logger;
  private readonly ReaderDefinition readerDefinition;
  private readonly TagReaderOptions options;

  private CancellationTokenSource? cts;
  private Task? readingTask;

  public InventoryTagReader(
    ILogger<InventoryTagReader> logger,
    ReaderDefinition readerDefinition,
    TagReaderOptions options)
  {

    ArgumentNullException.ThrowIfNull(readerDefinition, nameof(readerDefinition));
    ArgumentNullException.ThrowIfNull(options, nameof(options));

    this.readerDefinition = readerDefinition;
    this.options = options;
    this.logger = logger;
  }

  public bool IsRunning => this.readingTask?.Status < TaskStatus.RanToCompletion;

  public async Task<TagReaderChannels> StartReadingAsync(CancellationToken token = default)
  {
    if (!this.readerDefinition.IsConnected)
      throw new Exception(("Reader Not connected."));

    if (this.IsRunning)
      throw new Exception("Reader Task Already Running.");

    var dataChannel = Channel.CreateUnbounded<TagReaderDataReport>(
      new UnboundedChannelOptions
      {
        SingleReader = true,
        SingleWriter = true,
      });

    var statusChannel = Channel.CreateUnbounded<TagReaderProcessStatusUpdate>(
      new UnboundedChannelOptions
      {
        SingleReader = true,
        SingleWriter = true,
      });

    this.cts = CancellationTokenSource.CreateLinkedTokenSource(token);

    _ = Task.Run(
     async () => await ExecuteAsync(dataChannel.Writer, statusChannel.Writer, cts.Token),
     token)
      .ContinueWith(t =>
      {
        if (t.IsFaulted)
        {
          var exceptions = t.Exception.Flatten().InnerExceptions;

          foreach (var ex in exceptions)
          {
            var messageFormat = "Exception in ExecuteAsync {type} {message}";
            this.logger.LogError(messageFormat, ex.GetType(), ex.Message);
          }
        }
      });

    return new TagReaderChannels(dataChannel.Reader, statusChannel.Reader);
  }

  public async Task StopReadingAsync(CancellationToken token = default)
  {
    if (this.cts is not null)
    {
      await this.cts.CancelAsync();
    }
  }

  private async Task ExecuteAsync(
    ChannelWriter<TagReaderDataReport> dataWriter,
    ChannelWriter<TagReaderProcessStatusUpdate> statusWriter,
    CancellationToken token)
  {
    try
    {
      this.readingTask = RunAsync(dataWriter, token);

      await statusWriter.WriteAsync(TagReaderProcessStatusUpdate.Started(), token);

      await this.readingTask;

      var statusUpdate =
        new TagReaderProcessStatusUpdate(
          "Reading Finished",
          TagReaderProcessState.Complete);

      await statusWriter.WriteAsync(statusUpdate);
    }
    catch (OperationCanceledException ex)
    {
      var statusUpdate =
        new TagReaderProcessStatusUpdate(
          ex.Message,
          TagReaderProcessState.Canceled);

      await statusWriter.WriteAsync(statusUpdate);
    }
    catch (Exception ex)
    {
      var statusUpdate =
        new TagReaderProcessStatusUpdate(
          ex.Message,
          TagReaderProcessState.Faulted);

      await statusWriter.WriteAsync(statusUpdate);
    }
    finally
    {
      dataWriter.Complete();
      statusWriter.Complete();
    }
  }

  private async Task RunAsync(
    ChannelWriter<TagReaderDataReport> dataWriter,
    CancellationToken token)
  {
    this.readerDefinition.ReaderModule.hm().setUsageMode(UsageMode.UseQueue);

    var inventoryParams = new InventoryParam();

    inventoryParams.setAntennas(this.options.Antennas);

    var reader = this.readerDefinition.ReaderModule;

    while (true)
    {
      token.ThrowIfCancellationRequested();

      var status = reader.hm().inventory(true, inventoryParams);

      if (status != ReaderStatus.Ok)
      {
        await HandleStatus_ThrowIfError(dataWriter, status, token);
        continue;
      }

      var tagList = new List<TagEntry>();

      while (reader.hm().queueItemCount() > 0)
      {
        var tagItem = reader.hm().popItem();

        if (tagItem is null)
          continue;

        tagList.Add(new TagEntry(tagItem));
      }

      var dataReport =
        new TagReaderDataReport(tagList, $"{tagList.Count} tag reads.");

      await dataWriter.WriteAsync(dataReport, token);
    }
  }

  private async Task HandleStatus_ThrowIfError(
    ChannelWriter<TagReaderDataReport> dataWriter,
    int status, CancellationToken token = default)
  {
    string message = this.readerDefinition.ReaderModule.lastErrorStatusText();

    if (status < ErrorCode.Ok)
    {
      throw new Exception(message);
    }

    var report = new TagReaderDataReport(message);
    await dataWriter.WriteAsync(report, token);
  }
}
