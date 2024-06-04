namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Controls;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

using Microsoft.Extensions.Logging;

public class InventoryViewModel : ViewModel,
  IInventoryViewModel,
  IRecipient<ReaderConnected>,
  IRecipient<ReaderDisconnected>,
  IDisposable
{
  private readonly ILogger<InventoryViewModel> logger;
  private readonly ITagReaderFactory tagReaderFactory;
  private readonly INavigationService navigationService;
  private readonly IMessenger messenger;

  private ITagReader? tagReader;

  private bool isReaderConnected;
  private bool clearOnStart;
  private bool ant1 = false;
  private bool ant2 = false;
  private bool ant3 = false;
  private bool ant4 = false;

  private Task readTask = Task.CompletedTask;

  public InventoryViewModel(
    ILogger<InventoryViewModel> logger,
    IMessenger messenger,
    ITagReaderFactory tagReaderFactory,
    INavigationService navigationService)
  {

    this.logger = logger;
    this.tagReaderFactory = tagReaderFactory;
    this.navigationService = navigationService;
    this.messenger = messenger;

    ClearOnStart = true;
    TagList = [];
    PollingFeedback = [];

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

    this.DeleteSelectedItems = new RelayCommand<object>(DeleteSelectedItemsExecute, DelectSelectedItemsCanExecute);
  }



  public bool IsReaderConnected
  {
    get => isReaderConnected;
    private set
    {
      OnPropertyChanging(nameof(IsReaderDisconnected));
      SetProperty(ref isReaderConnected, value);
      OnPropertyChanged(nameof(IsReaderDisconnected));
      OnInventoryTaskCanExecuteChanged();
    }
  }

  public bool IsReaderDisconnected => !IsReaderConnected;

  public ObservableTagEntryCollection TagList { get; }

  public ObservableCollection<PollingFeedback> PollingFeedback { get; }
  public bool ClearOnStart
  {
    get => this.clearOnStart;
    set => SetProperty(ref this.clearOnStart, value);
  }

  public bool Antenna1 { get => this.ant1; set => SetProperty(ref this.ant1, value); }
  public bool Antenna2 { get => this.ant2; set => SetProperty(ref this.ant2, value); }
  public bool Antenna3 { get => this.ant3; set => SetProperty(ref this.ant3, value); }
  public bool Antenna4 { get => this.ant4; set => SetProperty(ref this.ant4, value); }
  public IRelayCommand ClearTagList { get; private set; }
  public IAsyncRelayCommand StartInventoryAsync { get; private set; }
  public IAsyncRelayCommand StopInventoryAsync { get; private set; }
  public IRelayCommand OpenSettings { get; private set; }
  public IRelayCommand<object> DeleteSelectedItems { get; private set; }

  private void ClearTagListExecute()
  {
    if (TagList.Any())
      TagList.Clear();
  }

  private void DeleteSelectedItemsExecute(object? obj)
  {
    if (obj is not IList selectedItems)
    {
      return;
    }

    var tagEntries = new ObservableTagEntry[selectedItems.Count];

    selectedItems.CopyTo(tagEntries, 0);

    foreach (var item in tagEntries)
    {
      var tagEntryToRemove = (ObservableTagEntry)item;
      this.TagList.Remove(tagEntryToRemove);
    }
  }

  private bool DelectSelectedItemsCanExecute(object? obj)
  {
    return true;
  }

  private void NavigateToSettingsMenuExecute()
  {
    navigationService.NavigateTo<ISettingsViewModel>();
  }

  private async Task StartInventoryExecuteAsync()
  {
    if (!StartInventoryCanExecute())
      return;

    var options = TagReaderOptions.Create(TagReaderMode.HostMode);

    byte antennas = 0x00;

    if (this.Antenna1) { antennas |= 0x01; }
    if (this.Antenna2) { antennas |= 0x02; }
    if (this.Antenna3) { antennas |= 0x04; }
    if (this.Antenna4) { antennas |= 0x08; }

    options.UseAntennas(antennas);

    this.tagReader = this.tagReaderFactory.Create(options);

    try
    {
      if (this.ClearOnStart)
        ClearTagListExecute();

      var channelReaders = await this.tagReader.StartReadingAsync();

      var readDataTask = ReadDataChannel(channelReaders.DataChannel);
      var readStatusTask = ReadStatusChannel(channelReaders.StatusChannel);

      await Task.WhenAll(readDataTask, readStatusTask);
    }
    catch (Exception ex)
    {
      //TODO: Display Error Status.
    }
  }

  private async Task ReadDataChannel(ChannelReader<TagReaderDataReport> dataChannel)
  {
    await foreach (var data in dataChannel.ReadAllAsync())
    {
      if (data.HasMessage)
      {
        this.PollingFeedback.Add(new PollingFeedback(data.Message));
      }

      foreach (var tagEntry in data.Tags)
      {
        this.TagList.Add(new ObservableTagEntry(tagEntry));
      }
    }
  }

  private async Task ReadStatusChannel(ChannelReader<TagReaderProcessStatusUpdate> statusChannel)
  {
    await foreach (var data in statusChannel.ReadAllAsync())
    {
      switch (data.State)
      {
        case TagReaderProcessState.Started:
          break;
        case TagReaderProcessState.Running:
          break;
        case TagReaderProcessState.Complete:
          break;
        case TagReaderProcessState.Canceled:
          break;
        case TagReaderProcessState.Faulted:
          break;
        default:
          break;
      }

      DispatcherHelper.CheckBeginInvokeOnUI(OnInventoryTaskCanExecuteChanged);
    }
  }


  private async Task StopInventoryExecuteAsync()
  {
    if (this.tagReader is not null)
      await this.tagReader.StopReadingAsync();
  }

  private bool StartInventoryCanExecute()
  {
    if (this.tagReader is not null && this.tagReader.IsRunning)
    {
      return false;
    }

    return IsReaderConnected;
  }

  private bool StopInventoryCanExecute()
  {
    if (this.tagReader is not null && this.tagReader.IsRunning)
      return true;

    return false;
  }

  private void OnInventoryTaskCanExecuteChanged()
  {
    DispatcherHelper.CheckBeginInvokeOnUI(() =>
    {
      StartInventoryAsync.NotifyCanExecuteChanged();
      StopInventoryAsync.NotifyCanExecuteChanged();
    });
  }

  public void Dispose()
  {
    messenger.UnregisterAll(this);
  }

  public void Receive(ReaderConnected message)
  {
    if (message.IsSelectedReader && message.ReaderDefinition.IsConnected)
    {
      this.IsReaderConnected = true;
      DispatcherHelper.CheckBeginInvokeOnUI(OnInventoryTaskCanExecuteChanged);
    }
  }

  public void Receive(ReaderDisconnected message)
  {
    if (message.IsSelectedReader && !message.ReaderDefinition.IsConnected)
    {
      this.IsReaderConnected = false;
      DispatcherHelper.CheckBeginInvokeOnUI(OnInventoryTaskCanExecuteChanged);
    }
  }
}
