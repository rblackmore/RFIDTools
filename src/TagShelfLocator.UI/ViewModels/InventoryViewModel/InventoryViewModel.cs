namespace TagShelfLocator.UI.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Core.Model;
using TagShelfLocator.UI.Helpers;
using TagShelfLocator.UI.Services;
using TagShelfLocator.UI.Services.InventoryService;
using TagShelfLocator.UI.Services.InventoryService.Events;
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

  private Task readTask = Task.CompletedTask;

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

    this.ClearTagList =
      new RelayCommand(ClearTagListExecute);

    this.OpenSettings =
      new RelayCommand(NavigateToSettingsMenuExecute);

    this.StartInventoryAsync =
      new AsyncRelayCommand(
        StartInventoryExecuteAsync,
        StartInventoryCanExecute);

    this.StopInventoryAsync =
      new AsyncRelayCommand(
        StopInventoryExecuteAsync,
        StopInventoryCanExecute);
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
  public IRelayCommand ClearTagList { get; private set; }
  public IAsyncRelayCommand StartInventoryAsync { get; private set; }
  public IAsyncRelayCommand StopInventoryAsync { get; private set; }
  public IRelayCommand OpenSettings { get; private set; }

  private void ClearTagListExecute()
  {
    if (this.TagList.Any())
      this.TagList.Clear();
  }

  private void NavigateToSettingsMenuExecute()
  {
    this.navigationService.NavigateTo<ISettingsViewModel>();
  }

  private async Task StartInventoryExecuteAsync()
  {
    var channel = Channel.CreateUnbounded<TagEntry>();

    await this.tagInventoryService.StartAsync(channel);

    this.readTask = ReadChannelAsync(channel.Reader);
  }

  private async Task StopInventoryExecuteAsync()
  {
    await this.tagInventoryService.StopAsync("Stop Requested by User");
  }

  private async Task CancelInventoryChannelReaderAsync()
  {
    if (this.readTask is null)
      return;

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

  // I should perhaps extract this out to a new class.
  // Or Change the InventoryService to Send a Message instead of using the Channel.
  private async Task ReadChannelAsync(
  ChannelReader<TagEntry> channelReader,
  CancellationToken cancellationToken = default)
  {
    while (await channelReader.WaitToReadAsync())
    {
      var tag = await channelReader.ReadAsync();

      DispatcherHelper.CheckBeginInvokeOnUI(() =>
      {
        this.TagList.Add(tag);
      });
    }
  }

  private void OnInventoryTaskCanExecuteChanged()
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
