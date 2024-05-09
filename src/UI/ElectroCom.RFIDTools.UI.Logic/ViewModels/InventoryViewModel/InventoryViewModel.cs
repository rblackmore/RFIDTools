namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Input;

using ElectroCom.RFIDTools.ReaderServices;

using Microsoft.Extensions.Logging;

public class InventoryViewModel : ViewModel,
  IInventoryViewModel,
  IDisposable
{
  private readonly ILogger<InventoryViewModel> logger;
  private readonly ITagReaderFactory tagReaderFactory;
  private readonly INavigationService navigationService;

  private ITagReader? tagReader;

  private bool isReaderConnected;
  private bool clearOnStart;

  private Task readTask = Task.CompletedTask;

  public InventoryViewModel(
    ILogger<InventoryViewModel> logger,
    ITagReaderFactory tagReaderFactory,
    INavigationService navigationService)
  {
    ClearOnStart = true;
    this.logger = logger;
    this.tagReaderFactory = tagReaderFactory;
    this.navigationService = navigationService;
    TagList = new();


    ClearTagList =
      new RelayCommand(ClearTagListExecute);

    OpenSettings =
      new RelayCommand(NavigateToSettingsMenuExecute);

    StartInventoryAsync =
      new AsyncRelayCommand(
        StartInventoryExecuteAsync,
        StartInventoryCanExecute);

    StopInventoryAsync =
      new AsyncRelayCommand(
        StopInventoryExecuteAsync,
        StopInventoryCanExecute);
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

  public bool ClearOnStart
  {
    get => clearOnStart;
    set => SetProperty(ref clearOnStart, value);
  }
  public IRelayCommand ClearTagList { get; private set; }
  public IAsyncRelayCommand StartInventoryAsync { get; private set; }
  public IAsyncRelayCommand StopInventoryAsync { get; private set; }
  public IRelayCommand OpenSettings { get; private set; }

  private void ClearTagListExecute()
  {
    if (TagList.Any())
      TagList.Clear();
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
    //TODO: Allow VM to select Antennas and call options.UseAntennas(byte);

    this.tagReader = this.tagReaderFactory.Create(options);

    try
    {
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
    //TODO: Display data.Message information, so I know if there is RF Warning or something else wrong.
    await foreach (var data in dataChannel.ReadAllAsync())
    {
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
      //TODO: Handle some different status.
      //Started (May need to add), Faulted, Canceled, Completed.
      //May all require OnProperteryChanged events for Is Running Property etc.
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
    StartInventoryAsync.NotifyCanExecuteChanged();
    StopInventoryAsync.NotifyCanExecuteChanged();
  }

  public void Dispose()
  {
    //messenger.UnregisterAll(this);
  }
}
