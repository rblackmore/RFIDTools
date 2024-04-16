namespace TagShelfLocator.UI.Services.ReaderManagement.Model;

using FEDM;

public enum CommunicationInterface
{
  None,
  USB,
  TCP,
  COM
}

public class ReaderDescription
{
  public ReaderDescription(ReaderModule readerModule)
    : this(readerModule, CommunicationInterface.None) { }

  public ReaderDescription(
    ReaderModule readerModule,
    CommunicationInterface communicationInterface)
  {
    this.CommunicationInterface = communicationInterface;
    this.ReaderModule = readerModule;

    this.DeviceID = this.ReaderModule.info().deviceId();
    this.DeviceName = this.ReaderModule.info().readerTypeToString();
  }

  public event ReaderConnectedEventHandler? ReaderConnected;
  public event ReaderDisconnectingEventHandler? ReaderDisconnecting;
  public event ReaderDisconnectedEventHandler? ReaderDisconnected;

  public ReaderModule ReaderModule { get; private init; }
  public uint DeviceID { get; private init; }
  public string DeviceName { get; private init; }

  public CommunicationInterface CommunicationInterface { get; private set; }

  public bool IsConnected => this.ReaderModule.isConnected();

  public bool Connect()
  {
    var isConnected = false;

    switch (this.CommunicationInterface)
    {
      case CommunicationInterface.USB:
        isConnected = ConnectUSB();
        break;
      case CommunicationInterface.TCP:
        isConnected = ConnectTCP();
        break;
      case CommunicationInterface.COM:
        isConnected = ConnectCOM();
        break;
      default:
        return false;
    }

    if (isConnected)
      OnReaderConnected();

    return isConnected;
  }

  public bool Disconnect()
  {
    OnDisconnecting();
    if (this.ReaderModule.isConnected())
      this.ReaderModule.disconnect();

    if (!this.IsConnected)
      OnDisconnected();

    return !this.IsConnected;
  }

  private void OnDisconnected()
  {
    this.ReaderDisconnected?.Invoke(this, new ReaderDisconnectedEventArgs(this.DeviceID));
  }

  private void OnDisconnecting()
  {
    // TODO: Consider MediatR,
    // it might have a better messenger implementation for notifying,
    // and waitin for all recipients to complete.
    this.ReaderDisconnecting?.Invoke(this, new ReaderDisconnectingEventArgs(this.DeviceID));
  }

  private void OnReaderConnected()
  {
    var deviceId = this.ReaderModule.info().deviceId();
    var deviceName = this.ReaderModule.info().readerTypeToString();

    this.ReaderConnected?.Invoke(this, new ReaderConnectedEventArgs(deviceId, deviceName));
  }

  private bool ConnectUSB()
  {
    var usbConnector = Connector.createUsbConnector(this.DeviceID);
    var status = this.ReaderModule.connect(usbConnector);

    if (status == ErrorCode.Ok)
      return true;

    return false;
  }

  private bool ConnectCOM()
  {
    return false;
  }

  private bool ConnectTCP()
  {
    return false;
  }

  public static ReaderDescription CreateUSB(ReaderModule reader)
    => new ReaderDescription(reader, CommunicationInterface.USB);
}
