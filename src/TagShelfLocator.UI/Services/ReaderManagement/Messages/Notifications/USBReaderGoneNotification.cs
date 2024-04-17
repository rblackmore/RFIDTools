namespace TagShelfLocator.UI.Services.ReaderManagement;

using MediatR;

public class USBReaderGone : INotification
{
  public USBReaderGone(uint deviceID)
  {
    this.DeviceID = deviceID;
  }

  public uint DeviceID { get; set; }
}
