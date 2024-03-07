namespace TagShelfLocator.UI.Services.ReaderConnectionListenerService.Messages;

public class ReaderConnected
{
  public ReaderConnected(uint deviceID, string deviceName)
  {
    this.DeviceID = deviceID;
    this.DeviceName = deviceName;
  }

  public uint DeviceID { get; }
  public string DeviceName { get; }
}
