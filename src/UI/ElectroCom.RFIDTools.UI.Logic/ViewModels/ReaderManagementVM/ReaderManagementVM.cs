namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

// TODO: Okay, I should update this to be a full Reader Selection ViewModel.
// Update the view to match as well.
// Display the currently selected Reader Details, included conneciton Status.
public class ReaderManagementVM : ViewModel,
  IDisposable,
  IReaderManagementVM,
  IRecipient<SelectedReaderChanged>,
  IRecipient<ReaderConnected>,
  IRecipient<ReaderDisconnected>
{

  private readonly IReaderManager readerManager;
  private readonly IMessenger messenger;

  public ReaderManagementVM(IReaderManager readerManager, IMessenger messenger)
  {
    this.IsConnected = false;
    this.DeviceName = string.Empty;
    this.DeviceID = 0;
    this.readerManager = readerManager;
    this.messenger = messenger;
    this.messenger.RegisterAll(this);

    this.ConnectReader =
      new RelayCommand(ConnectReaderExecute, ConnectReaderCanExecute);

    this.DisconnectReader =
      new RelayCommand(DisconnectReaderExecute, DisconnectReaderCanExecute);
  }

  private bool isConnected;
  public bool IsConnected
  {
    get => isConnected;
    set => SetProperty(ref this.isConnected, value);
  }

  private string deviceName = string.Empty;
  public string DeviceName
  {
    get => deviceName;
    set => SetProperty(ref this.deviceName, value);
  }

  private uint deviceID;

  public uint DeviceID
  {
    get => deviceID;
    set => SetProperty(ref this.deviceID, value);
  }

  public IRelayCommand ConnectReader { get; private set; }

  public IRelayCommand DisconnectReader { get; private set; }

  private void ConnectReaderExecute()
  {
    var rd = this.readerManager.SelectedReader;

    if (!ConnectReaderCanExecute())
      return;

    rd.Connect();
  }

  private bool ConnectReaderCanExecute()
  {
    var rd = this.readerManager.SelectedReader;

    if (rd is NullReaderDefinition)
      return false;

    if (rd.IsConnected)
      return false;

    return true;
  }

  private void DisconnectReaderExecute()
  {
    var rd = this.readerManager.SelectedReader;

    if (!DisconnectReaderCanExecute())
      return;

    rd.Disconnect();
  }

  private bool DisconnectReaderCanExecute()
  {
    var rd = this.readerManager.SelectedReader;

    if (rd is NullReaderDefinition)
      return false;

    if (rd.IsConnected)
      return true;

    return false;
  }

  public void Receive(SelectedReaderChanged message)
  {
    this.DeviceID = message.DeviceID;
    this.DeviceName = message.DeviceName;
    this.IsConnected = message.IsConnected;
    ConnectReaderCanExecuteChanged();
  }

  public void Receive(ReaderConnected message)
  {
    if (!message.IsSelectedReader)
      return;

    this.IsConnected = message.ReaderDefinition.IsConnected;
    ConnectReaderCanExecuteChanged();
  }

  public void Receive(ReaderDisconnected message)
  {
    if (!message.IsSelectedReader)
      return;

    this.IsConnected = message.ReaderDefinition.IsConnected;
    ConnectReaderCanExecuteChanged();
  }

  private void ConnectReaderCanExecuteChanged()
  {
    DispatcherHelper.CheckBeginInvokeOnUI(() =>
    {
      this.ConnectReader.NotifyCanExecuteChanged();
      this.DisconnectReader.NotifyCanExecuteChanged();
    });
  }

  public void Dispose()
  {
    this.messenger.UnregisterAll(this);
  }
}
