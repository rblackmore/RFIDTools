namespace TagShelfLocator.UI.Services;

using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using FEDM;

using Microsoft.Extensions.Logging;

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

  public Task StartAsync(Channel<TagReadViewModel> channel, CancellationToken cancellationToken = default)
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

  public async Task RunAsync(ChannelWriter<TagReadViewModel> channelWriter, CancellationToken cancellationToken = default)
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

        await channelWriter.WriteAsync(new TagReadViewModel(tagItem.iddToHexString()));

        tagItem.clear();
      }
    }
  }
}


public class TagReadViewModel : ObservableObject
{
  private string idd = string.Empty;

  public TagReadViewModel(string iDD)
  {
    IDD = iDD;
  }

  public string IDD
  {
    get => this.idd;
    set => SetProperty(ref this.idd, value);
  }
}
