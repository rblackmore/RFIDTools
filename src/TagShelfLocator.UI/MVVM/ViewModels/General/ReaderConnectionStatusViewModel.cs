namespace TagShelfLocator.UI.MVVM.ViewModels;

using CommunityToolkit.Mvvm.Messaging;

public class ReaderConnectionStatusViewModel : ViewModel,
  IReaderConnectionStatusViewModel
{
  private readonly IMessenger messenger;

  public ReaderConnectionStatusViewModel(IMessenger messenger)
  {
    this.IsConnected = false;
    this.ReaderName = string.Empty;
    this.DeviceID = 0;
    this.messenger = messenger;

    this.messenger.RegisterAll(this);
  }

  private bool isConnected;
  public bool IsConnected
  {
    get { return isConnected; }
    set
    {
      isConnected = value;
      OnPropertyChanged();
    }
  }

  private string readerName;
  public string ReaderName
  {
    get { return readerName; }
    set
    {
      readerName = value;
      OnPropertyChanged();
    }
  }

  private uint deviceID;

  public uint DeviceID
  {
    get { return deviceID; }
    set
    {
      deviceID = value;
      OnPropertyChanged();
    }
  }


  //public void Receive(ReaderConnected message)
  //{
  //  this.IsConnected = true;
  //  this.ReaderName = message.DeviceName;
  //  this.DeviceID = message.DeviceID;
  //}

  //public void Receive(ReaderDisconnected message)
  //{
  //  this.IsConnected = false;
  //  this.ReaderName = "No Reader\nConnected";
  //  this.DeviceID = 0;
  //}
}
