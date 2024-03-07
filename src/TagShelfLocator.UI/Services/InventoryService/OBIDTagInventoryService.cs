namespace TagShelfLocator.UI.Services.InventoryService;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using FEDM;
using FEDM.TagHandler;

using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Core.Model;
using TagShelfLocator.UI.Services;
using TagShelfLocator.UI.Services.InventoryService.Events;

// TODO: Eventually intend to extract an Interface from this,
// so i can create different versions for different readers.
public class OBIDTagInventoryService : ITagInventoryService
{
  private readonly ILogger<OBIDTagInventoryService> logger;
  private readonly IMessenger messenger;
  private readonly ReaderModule reader;

  private Task RunningTask = Task.CompletedTask;

  private CancellationTokenSource cancellationTokenSource = new();

  public OBIDTagInventoryService(ILogger<OBIDTagInventoryService> logger, IMessenger messenger, ReaderModule reader)
  {
    this.logger = logger;
    this.messenger = messenger;
    this.reader = reader;

    this.messenger.Register<ReaderDisconnecting>(this, async (r, m) =>
    {
      m.RunningTask = RunningTask;
      await StopAsync("Reader Disconnected");
    });
  }
  //public event InventoryStoppedHandler InventoryStopped;

  public bool IsRunning => !IsNotRunning;

  public bool IsNotRunning => RunningTask is null || RunningTask.IsCompleted;

  public async Task StartAsync(Channel<TagEntry> channel, string message = "", CancellationToken cancellationToken = default)
  {
    if (IsRunning)
      return;

    cancellationTokenSource = new CancellationTokenSource();

    RunningTask = Task.Run(async () =>
    {
      await RunAsync(channel.Writer, cancellationTokenSource.Token);
    })
    .ContinueWith(async tsk =>
    {

      if (!tsk.IsFaulted)
        return;

      await StopAsync("Exception in Running Task");

      if (tsk.Exception is null)
        return;

      foreach (var ex in tsk.Exception.Flatten().InnerExceptions)
      {
        logger.LogError("Exception in Running Task {exType} {exMessage}", ex.GetType(), ex.Message);
      }
    });

    this.messenger.Send(new InventoryStartedMessage(message));
  }

  public async Task StopAsync(string message, CancellationToken cancellationToken = default)
  {
    if (IsNotRunning)
      return;

    cancellationTokenSource?.Cancel();
    await RunningTask;
    this.messenger.Send(new InventoryStoppedMessage(message));
  }

  public async Task RunAsync(ChannelWriter<TagEntry> channelWriter, CancellationToken cancellationToken = default)
  {
    reader.hm().setUsageMode(Hm.UsageMode.UseQueue);

    while (!cancellationToken.IsCancellationRequested)
    {
      var state = reader.hm().inventory();

      // TODO: I should handle a few other error codes depending on what may go wrong.
      // eg. Code 0x01 means no tags, this is fine to continue
      // Code 0x84 Means RF-Warning, the reader has noise issues, perhaps I should stop the loop,
      // or display the error and continue anyway.
      if (state != ErrorCode.Ok)
      {
        logger.LogError("{status} - {message}", state, reader.lastErrorStatusText());
        continue;
      }

      var count = 0;

      while (reader.hm().queueItemCount() > 0)
      {
        var tagItem = reader.hm().popItem();

        if (tagItem is null)
          continue;

        var tagEntry = await TagEntry.FromOBIDTagItem(count, tagItem);
        await channelWriter.WriteAsync(tagEntry);

        tagItem.clear();
      }
    }
  }

  private EPCTagEntry CreateEPCTagEntry(TagItem tagItem)
  {
    var th = reader.hm().createTagHandler(tagItem);

    if (th is not ThEpcClass1Gen2 thEPC)
      throw new System.Exception("Tag Item is not EpcClass2Gen2 Tag");


    var rssiValues = new List<Antenna>();

    foreach (var rssi in tagItem.rssiValues())
      rssiValues.Add(new Antenna(rssi.antennaNumber(), rssi.rssi()));

    return new EPCTagEntry(
      thEPC.idd(),
      thEPC.epcToHexString(),
      thEPC.tidToHexString(),
      rssiValues.AsReadOnly());
  }
}
