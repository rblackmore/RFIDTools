namespace ElectroCom.RFIDTools.ReaderServices;

using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using ElectroCom.RFIDTools.ReaderServices.Model;

using FEDM;

using static FEDM.Hm;

public class InventoryTagReader : ITagReader
{
  private readonly ReaderDefinition readerDefinition;
  private readonly TagReaderOptions options;

  private CancellationTokenSource? cts;
  private Task? readingTask;

  public InventoryTagReader(
    ReaderDefinition readerDefinition,
    TagReaderOptions options)
  {

    ArgumentNullException.ThrowIfNull(readerDefinition, nameof(readerDefinition));
    ArgumentNullException.ThrowIfNull(options, nameof(options));

    this.readerDefinition = readerDefinition;
    this.options = options;
  }

  public bool IsRunning => this.readingTask?.Status < TaskStatus.RanToCompletion;

  public async Task<TagReaderChannels> StartReadingAsync(CancellationToken token = default)
  {
    if (!this.readerDefinition.IsConnected)
    {
      throw new Exception(("Reader Not connected."));
    }

    if (this.IsRunning)
    {
      throw new Exception("Reader Task Already Running.");
    }

    var dataChannel = Channel.CreateUnbounded<TagReadDataReport>(
      new UnboundedChannelOptions
      {
        SingleReader = true,
        SingleWriter = true,
      });

    var statusChannel = Channel.CreateUnbounded<TagReaderTaskStatusUpdate>(
      new UnboundedChannelOptions
      {
        SingleReader = true,
        SingleWriter = true,
      });

    this.cts = CancellationTokenSource.CreateLinkedTokenSource(token);

    await Task.Factory.StartNew(
      async () => await ExecuteAsync(dataChannel.Writer, statusChannel.Writer, cts.Token),
      token,
      TaskCreationOptions.LongRunning,
      TaskScheduler.Default);

    return new TagReaderChannels(dataChannel.Reader, statusChannel.Reader);
  }

  public async Task StopReadingAsync(CancellationToken token = default)
  {
    if (this.readingTask.IsCompleted)
    {
      return;
    }

    this.cts?.Cancel();

    await this.readingTask;
  }

  private async Task ExecuteAsync(
    ChannelWriter<TagReadDataReport> dataWriter,
    ChannelWriter<TagReaderTaskStatusUpdate> statusWriter,
    CancellationToken token)
  {
    //TODO: Catch FEDM Exceptions, and turn them into something meaningful.
    try
    {
      this.readingTask = RunAsync(dataWriter, token);
      await this.readingTask;
      await statusWriter.WriteAsync(new TagReaderTaskStatusUpdate("Reading Finished", true, false, false));
    }
    catch (OperationCanceledException ex)
    {
      await statusWriter.WriteAsync(new TagReaderTaskStatusUpdate(ex.Message, true, true, true));
    }
    catch (Exception ex)
    {
      await statusWriter.WriteAsync(new TagReaderTaskStatusUpdate(ex.Message, false, true, false));
    }
    finally
    {
      dataWriter.Complete();
      statusWriter.Complete();
    }
  }

  private Task RunAsync(ChannelWriter<TagReadDataReport> dataWriter, CancellationToken token)
  {
    this.readerDefinition.ReaderModule.hm().setUsageMode(UsageMode.UseQueue);

    var inventoryParams = new InventoryParam();

    inventoryParams.setAntennas(this.options.Antennas);

    var reader = this.readerDefinition.ReaderModule;

    while (true)
    {
      token.ThrowIfCancellationRequested();

      var status = reader.hm().inventory(true, inventoryParams);

      if (status != ErrorCode.Ok)
      {
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
    }
  }
}
