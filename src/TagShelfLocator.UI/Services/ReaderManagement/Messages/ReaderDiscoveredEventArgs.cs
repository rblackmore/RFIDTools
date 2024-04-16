namespace TagShelfLocator.UI.Services.ReaderManagement;
using FEDM;

public class USBReaderDiscovered
{
  public USBReaderDiscovered(ReaderModule reader, uint deviceID)
  {
    Reader = reader;
    DeviceID = deviceID;
  }
  public ReaderModule Reader { get; init; }
  public uint DeviceID { get; set; }
}
