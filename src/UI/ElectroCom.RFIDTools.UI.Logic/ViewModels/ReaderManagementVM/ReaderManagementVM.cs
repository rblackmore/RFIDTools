namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

// TODO: Okay, I should update this to be a full Reader Selection ViewModel.
// Update the view to match as well.
// Display the currently selected Reader Details, included conneciton Status.
public class ReaderManagementVM : ViewModel,
  IReaderManagementVM,
  IRecipient<SelectedReaderChanged>
{

  private readonly IReaderManager readerManager;

  public ReaderManagementVM(IReaderManager readerManager)
  {
    this.IsConnected = false;
    this.ReaderName = string.Empty;
    this.DeviceID = 0;
    this.readerManager = readerManager;
  }

  private bool isConnected;
  public bool IsConnected
  {
    get => isConnected;
    set => SetProperty(ref this.isConnected, value);
  }

  private string? readerName;
  public string? ReaderName
  {
    get => readerName;
    set => SetProperty(ref this.readerName, value);
  }

  private uint deviceID;

  public uint DeviceID
  {
    get => deviceID;
    set => SetProperty(ref this.deviceID, value);
  }

  public void Receive(SelectedReaderChanged message)
  {
    this.DeviceID = message.DeviceName;
    this.ReaderName = message.ReaderName;
  }

  //public void SelectedReaderChanged(uint deviceId)
  //{
  //  this.readerDescription = this.readerManager.SelectedReader;

  //  if (!this.readerDescription.IsConnected)
  //    this.readerDescription.Connect();

  //  this.IsConnected = this.readerDescription.IsConnected;

  //  this.DeviceID = this.readerDescription.DeviceID;
  //  this.ReaderName = this.readerDescription.ReaderName;
  //}
}
