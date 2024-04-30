namespace OBID.Scratch;

using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using FEDM;

public class InventoryTagReadingWorker : ITagReadingWorker
{
  private ChannelWriter<List<TagItem>> channelWriter;
  private ReaderModule readerModule;

  private CancellationTokenSource? cancellationTokenSource;
  private Task runningTask = Task.CompletedTask;

  public InventoryTagReadingWorker(
    ChannelWriter<List<TagItem>> channelWriter,
    ReaderModule readerModule)
  {
    this.channelWriter = channelWriter;
    this.readerModule = readerModule;
  }

  public Task StartAsync(CancellationToken token = default)
  {
    if (!this.readerModule.isConnected())
      throw new Exception("Reader Not Connected Exception");

    if (!runningTask.IsCompleted)
      return Task.CompletedTask;

    cancellationTokenSource = new CancellationTokenSource();

    runningTask = Task.Run(async () =>
    {
      await RunAsync(cancellationTokenSource.Token);
    });

    return Task.CompletedTask;
  }

  public async Task StopAsync(CancellationToken token = default)
  {
    if (runningTask.IsCompleted)
      return;

    cancellationTokenSource?.Cancel();

    await runningTask;
  }


  private Task RunAsync(CancellationToken token)
  {
    this.readerModule.hm().setUsageMode(Hm.UsageMode.UseQueue);

    while (!token.IsCancellationRequested)
    {
      var state = this.readerModule.hm().inventory();

      if (state != ErrorCode.Ok)
        continue;

      var tagList = new List<TagItem>();

      while (this.readerModule.hm().queueItemCount() > 0)
      {
        var tagItem = this.readerModule.hm().popItem();

        if (tagItem is null)
          continue;

        tagList.Add(tagItem);
      }

      this.channelWriter.WriteAsync(tagList);
    }

    return Task.CompletedTask;
  }
}
