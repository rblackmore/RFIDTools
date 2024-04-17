namespace TagShelfLocator.UI.Services.ReaderManagement;
using MediatR;

public class USBReaderDiscovered : INotification
{
  public USBReaderDiscovered(uint deviceID, uint readerType)
  {
    DeviceID = deviceID;
    ReaderType = readerType;
  }

  public uint DeviceID { get; set; }
  public uint ReaderType { get; }
}
