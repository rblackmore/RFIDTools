namespace TagShelfLocator.UI.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using FEDM;
using FEDM.TagHandler;

using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Core.Model;

public class TagReaderService
{
  private readonly ILogger<TagReaderService> logger;
  private readonly IMessenger messenger;
  private readonly ReaderModule reader;

  private Task RunningTask = Task.CompletedTask;

  private CancellationTokenSource cancellationTokenSource = new();

  public TagReaderService(ILogger<TagReaderService> logger, IMessenger messenger, ReaderModule reader)
  {
    this.logger = logger;
    this.messenger = messenger;
    this.reader = reader;

    this.messenger.Register<ReaderDisconnecting>(this, async (r, m) =>
    {
      m.RunningTask = this.RunningTask;
      await StopAsync();
    });
  }

  public bool IsRunning => !this.IsNotRunning;

  public bool IsNotRunning => this.RunningTask is null || this.RunningTask.IsCompleted;

  public Task StartAsync(Channel<EPCTagEntry> channel, CancellationToken cancellationToken = default)
  {
    if (this.IsRunning)
      return Task.CompletedTask;

    this.cancellationTokenSource = new CancellationTokenSource();

    this.RunningTask = Task.Run(async () =>
    {
      await RunAsync(channel.Writer, this.cancellationTokenSource.Token);
    });

    return Task.CompletedTask;
  }

  public async Task StopAsync(CancellationToken cancellationToken = default)
  {
    if (this.IsNotRunning)
      return;

    this.cancellationTokenSource?.Cancel();
    await this.RunningTask;
  }

  public async Task RunAsync(ChannelWriter<EPCTagEntry> channelWriter, CancellationToken cancellationToken = default)
  {
    this.reader.hm().setUsageMode(Hm.UsageMode.UseQueue);

    var inventoryParams = new InventoryParam();

    inventoryParams.setAntennas(0x08);

    while (!cancellationToken.IsCancellationRequested)
    {
      int state = this.reader.hm().inventory(true, inventoryParams);

      if (state != ErrorCode.Ok)
        continue;

      while (this.reader.hm().queueItemCount() > 0)
      {
        var tagItem = this.reader.hm().popItem();

        if (tagItem is null)
          continue;

        await channelWriter.WriteAsync(CreateEPCTagEntry(tagItem));
        tagItem.clear();
      }
    }
  }

  private EPCTagEntry CreateEPCTagEntry(TagItem tagItem)
  {
    var th = this.reader.hm().createTagHandler(tagItem);

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
