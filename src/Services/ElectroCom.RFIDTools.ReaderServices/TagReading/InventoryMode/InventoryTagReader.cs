namespace ElectroCom.RFIDTools.ReaderServices.TagReaders;

using System;
using System.Threading;
using System.Threading.Tasks;

using ElectroCom.RFIDTools.ReaderServices.Model;
using ElectroCom.RFIDTools.ReaderServices.TagReaders.InventoryMode;

using FEDM;

using static FEDM.Hm;

public class InventoryTagReader : ITagReader
{
  private readonly ReaderDefinition readerDefinition;
  private readonly InventoryTagReaderOptions options;

  private CancellationTokenSource? cancellationTokenSource;
  private Task readingTask = Task.CompletedTask;

  public InventoryTagReader(
    ReaderDefinition readerDefinition,
    InventoryTagReaderOptions options = null!)
  {
    this.readerDefinition = readerDefinition;
    this.options = options ?? InventoryTagReaderOptions.Default;
  }

  public bool IsRunning => this.readingTask.Status < TaskStatus.RanToCompletion;

  public Task StartReadingAsync(CancellationToken token = default)
  {
    if (!this.readerDefinition.IsConnected)
    {
      throw new Exception(("Reader Not connected"));
    }

    if (!this.readingTask.IsCompleted)
    {
      return Task.CompletedTask;
    }

    this.cancellationTokenSource = new CancellationTokenSource();

    this.readingTask = RunAsync(cancellationTokenSource.Token)
      .ContinueWith(HandleCompletion);

    return Task.CompletedTask;
  }

  public async Task StopReadingAsync(CancellationToken token = default)
  {
    if (this.readingTask.IsCompleted)
    {
      return;
    }

    this.cancellationTokenSource?.Cancel();

    await this.readingTask;
  }

  private Task RunAsync(CancellationToken token)
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

  private Task HandleCompletion(Task t)
  {

  }
}
