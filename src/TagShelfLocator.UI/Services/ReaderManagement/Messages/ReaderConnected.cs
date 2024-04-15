namespace TagShelfLocator.UI.Services.ReaderManagement.Messages;

public class ReaderConnected
{
  public ReaderConnected(uint deviceID, string deviceName)
  {
    DeviceID = deviceID;
    DeviceName = deviceName;
  }

  public uint DeviceID { get; }
  public string DeviceName { get; }
}
