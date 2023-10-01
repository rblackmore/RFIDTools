namespace TagShelfLocator.UI.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using TagShelfLocator.UI.Helpers;
using TagShelfLocator.UI.Services;

public class MainViewModel : ObservableObject, IMainViewModel
{
  private readonly IMessenger messenger;
  private readonly TagReaderService tagReaderService;
  private bool isReaderConnected;

  public MainViewModel(IMessenger messenger, TagReaderService tagReaderService)
  {
    this.messenger = messenger;
    this.tagReaderService = tagReaderService;
    this.TagList = new();

    this.StartInventoryAsync =
      new AsyncRelayCommand(StartInventoryExecuteAsync, StartInventoryCanExecute);

    this.StopInventoryAsync =
      new AsyncRelayCommand(StopInventoryExecuteAsync, StopInventoryCanExecute);

    this.messenger.Register<ReaderConnectionStateChangedMessage>(this, async (r, m) =>
    {
      this.IsReaderConnected = m.NewConnectionStatus;

      if (!m.NewConnectionStatus)
        await StopInventoryExecuteAsync();
    });
  }

  public bool IsReaderConnected
  {
    get => this.isReaderConnected;
    private set
    {
      OnPropertyChanging(nameof(IsReaderDisconnected));
      SetProperty(ref this.isReaderConnected, value);
      OnPropertyChanged(nameof(IsReaderDisconnected));
      OnInventoryTaskCanExecuteChanged();
    }
  }

  public bool IsReaderDisconnected => !IsReaderConnected;

  public ObservableCollection<TagReadViewModel> TagList { get; }
  public IAsyncRelayCommand StartInventoryAsync { get; private set; }
  public IAsyncRelayCommand StopInventoryAsync { get; private set; }

  private CancellationTokenSource readTaskTokenSource = new();

  private Task readTask = Task.CompletedTask;

  private async Task StartInventoryExecuteAsync()
  {
    var channel = Channel.CreateUnbounded<TagReadViewModel>();
    await this.tagReaderService.StartAsync(channel);

    this.readTaskTokenSource = new();

    this.readTask = ReadChannelAsync(channel.Reader, readTaskTokenSource.Token);
    OnInventoryTaskCanExecuteChanged();
  }

  private async Task StopInventoryExecuteAsync()
  {
    if (this.readTask.IsCompleted)
      return;

    this.readTaskTokenSource.Cancel();
    await this.tagReaderService.StopAsync();
    await this.readTask;
    OnInventoryTaskCanExecuteChanged();
  }

  private bool StartInventoryCanExecute()
  {
    return this.IsReaderConnected && this.tagReaderService.IsNotRunning;
  }

  private bool StopInventoryCanExecute()
  {
    return this.tagReaderService.IsRunning;
  }

  private async Task ReadChannelAsync(
    ChannelReader<TagReadViewModel> channelReader,
    CancellationToken cancellationToken = default)
  {
    try
    {

      while (await channelReader.WaitToReadAsync(cancellationToken))
      {
        var tag = await channelReader.ReadAsync(cancellationToken);
        this.TagList.Add(tag);
      }
    }
    catch (OperationCanceledException)
    {
      // it's fine ignore this, it's to be expected.
    }
  }

  public void OnInventoryTaskCanExecuteChanged()
  {
    DispatcherHelper.CheckBeginInvokeOnUI(() =>
    {
      this.StartInventoryAsync.NotifyCanExecuteChanged();
      this.StopInventoryAsync.NotifyCanExecuteChanged();
    });
  }
}
