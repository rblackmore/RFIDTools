namespace TagShelfLocator.UI.Services.ReaderManagement;
public class USBReaderGone
{
  public USBReaderGone(uint deviceID)
  {
    DeviceID = deviceID;
  }

  public uint DeviceID { get; set; }
}
