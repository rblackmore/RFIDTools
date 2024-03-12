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
using TagShelfLocator.UI.Services.InventoryService.Messages;
using TagShelfLocator.UI.Services.ReaderConnectionListenerService.Messages;

public class InventoryViewModel : ViewModel,
  IInventoryViewModel,
  IDisposable,
  IRecipient<InventoryStartedMessage>,
  IRecipient<InventoryStoppedMessage>,
  IRecipient<ReaderConnected>,
  IRecipient<ReaderDisconnected>,
  IRecipient<InventoryTagItemsDetectedMessage>
{
  private readonly ILogger<InventoryViewModel> logger;
  private readonly IMessenger messenger;

  private readonly ITagInventoryService tagInventoryService;
  private readonly INavigationService navigationService;

  private bool isReaderConnected;
  private bool clearOnStart;

  private Task readTask = Task.CompletedTask;

  public InventoryViewModel(
    ILogger<InventoryViewModel> logger,
    IMessenger messenger,
    ITagInventoryService tagInventoryService,
    INavigationService navigationService)
  {
    this.ClearOnStart = true;
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
  public bool ClearOnStart
  {
    get => this.clearOnStart;
    set => SetProperty(ref this.clearOnStart, value);
  }
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
    if (this.ClearOnStart)
      this.ClearTagListExecute();

    await this.tagInventoryService.StartAsync();
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

  public void Receive(InventoryTagItemsDetectedMessage message)
  {
    DispatcherHelper.CheckBeginInvokeOnUI(() =>
    {
      this.ClearTagListExecute();

      foreach (var tag in message.Tags)
      {
        this.TagList.Add(tag);
      }
    });
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
