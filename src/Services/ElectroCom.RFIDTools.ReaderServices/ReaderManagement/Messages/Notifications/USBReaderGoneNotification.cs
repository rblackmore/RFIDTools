namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using MediatR;

public class USBReaderGone : INotification
{
  public USBReaderGone(uint deviceID)
  {
    this.DeviceID = deviceID;
  }

  public uint DeviceID { get; set; }
}
