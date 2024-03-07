namespace TagShelfLocator.UI.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using FEDM;

using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Core.Model;
using TagShelfLocator.UI.Helpers;
using TagShelfLocator.UI.Services;
using TagShelfLocator.UI.Services.InventoryService;
using TagShelfLocator.UI.Services.InventoryService.Events;
using TagShelfLocator.UI.Services.ReaderConnectionListenerService;
using TagShelfLocator.UI.Services.ReaderConnectionListenerService.Messages;

public class InventoryViewModel : ViewModel,
  IInventoryViewModel,
  IDisposable,
  IRecipient<InventoryStartedMessage>,
  IRecipient<InventoryStoppedMessage>,
  IRecipient<ReaderConnected>,
  IRecipient<ReaderDisconnected>
{
  private readonly ILogger<InventoryViewModel> logger;
  private readonly IMessenger messenger;
  private readonly ITagInventoryService tagInventoryService;
  private readonly INavigationService navigationService;
  private bool isReaderConnected;

  public InventoryViewModel(
    ILogger<InventoryViewModel> logger,
    IMessenger messenger,
    ITagInventoryService tagInventoryService,
    INavigationService navigationService)
  {
    this.logger = logger;
    this.messenger = messenger;
    this.tagInventoryService = tagInventoryService;
    this.navigationService = navigationService;
    this.TagList = new();

    this.messenger.RegisterAll(this);

    this.ConfigureCommands();
  }

  public async void Receive(ReaderConnected message)
  {
    this.IsReaderConnected = true;
  }

  public async void Receive(ReaderDisconnected message)
  {
    this.IsReaderConnected = false;
    await CancelInventoryChannelReaderAsync();
    this.OnInventoryTaskCanExecuteChanged();
  }

  public void Receive(InventoryStartedMessage message)
  {
    this.OnInventoryTaskCanExecuteChanged();
  }

  public async void Receive(InventoryStoppedMessage message)
  {
    await this.CancelInventoryChannelReaderAsync();
    this.OnInventoryTaskCanExecuteChanged();
  }

  private void ConfigureCommands()
  {
    this.StartInventoryAsync =
      new AsyncRelayCommand(StartInventoryExecuteAsync, StartInventoryCanExecute);

    this.StopInventoryAsync =
      new AsyncRelayCommand(StopInventoryExecuteAsync, StopInventoryCanExecute);

    this.OpenSettings = new RelayCommand(() =>
    {
      this.navigationService.NavigateTo<ISettingsViewModel>();
    });

    this.AddTagEntry = new RelayCommand(() =>
    {
      this.TagList.Add(new TagEntry(1, "DESFire", "1234", -17));
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

  public ObservableCollection<TagEntry> TagList { get; }
  public IAsyncRelayCommand StartInventoryAsync { get; private set; }
  public IAsyncRelayCommand StopInventoryAsync { get; private set; }
  public IRelayCommand OpenSettings { get; private set; }
  public IRelayCommand AddTagEntry { get; private set; }

  private CancellationTokenSource readTaskTokenSource = new();

  private Task readTask = Task.CompletedTask;

  private async Task StartInventoryExecuteAsync()
  {
    var channel = Channel.CreateUnbounded<TagEntry>();

    await this.tagInventoryService.StartAsync(channel);

    this.readTaskTokenSource = new();

    this.readTask = ReadChannelAsync(channel.Reader, readTaskTokenSource.Token);
  }

  private async Task StopInventoryExecuteAsync()
  {
    await this.tagInventoryService.StopAsync("Stop Requested by User");
  }

  private async Task CancelInventoryChannelReaderAsync()
  {
    if (this.readTask is null)
      return;

    this.readTaskTokenSource.Cancel();
    await this.readTask;
  }

  private bool StartInventoryCanExecute()
  {
    return this.IsReaderConnected && this.tagInventoryService.IsNotRunning;
  }

  private bool StopInventoryCanExecute()
  {
    return this.tagInventoryService.IsRunning;
  }

  private async Task ReadChannelAsync(
    ChannelReader<TagEntry> channelReader,
    CancellationToken cancellationToken = default)
  {
    try
    {

      while (await channelReader.WaitToReadAsync(cancellationToken))
      {
        var tag = await channelReader.ReadAsync(cancellationToken);

        DispatcherHelper.CheckBeginInvokeOnUI(() =>
        {
          this.TagList.Add(tag);
        });
      }
    }
    catch (OperationCanceledException ex)
    {
      this.logger.LogInformation("Read Channel Task Cancelled: {message}", ex.Message);
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

  public void Dispose()
  {
    this.messenger.UnregisterAll(this);
  }
}
