namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using System;

using FEDM;

public enum CommunicationInterface
{
  USB,
  TCP,
  COM
}

public class ReaderDescription
{
  private AppLoggingParam appLoggingParams;
  private CommunicationInterface communicationInterface;
  private ReaderModule readerModule;
  private uint deviceId;
  private uint readerType;

  public ReaderDescription(
    uint deviceId,
    uint readerType,
    CommunicationInterface communicationInterface)
  {
    if (deviceId == 0)
      throw new Exception($"Invalid deviceId({deviceId})");

    this.readerModule = new ReaderModule(RequestMode.UniDirectional);
    this.communicationInterface = communicationInterface;
    this.readerType = readerType;
    this.deviceId = deviceId;

    this.appLoggingParams = AppLoggingParam.createFileLogger($"{DeviceID}.log");
  }

  public ReaderModule ReaderModule => this.readerModule;
  public uint DeviceID => this.deviceId;
  public uint ReaderType => this.readerType;
  public string ReaderName => FEDM.ReaderType.toString(this.readerType);

  public CommunicationInterface CommunicationInterface => this.communicationInterface;

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

    return isConnected;
  }

  public bool Disconnect()
  {
    if (!this.ReaderModule.isConnected())
      return true;

    this.ReaderModule.disconnect();

    return !this.IsConnected;
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
    throw new NotImplementedException();
  }

  private bool ConnectTCP()
  {
    throw new NotImplementedException();
  }

  public string StartLogging()
  {
    this.ReaderModule.log().startLogging(appLoggingParams);
    return this.appLoggingParams.logFile();
  }

  public void StopLogging()
  {
    this.ReaderModule.log().stopLogging();
  }

  public static ReaderDescription CreateUSB(uint deviceId, uint readerType)
    => new ReaderDescription(deviceId, readerType, CommunicationInterface.USB);
}
