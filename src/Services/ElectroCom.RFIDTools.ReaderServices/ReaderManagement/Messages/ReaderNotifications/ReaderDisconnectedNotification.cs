namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using MediatR;

public class ReaderDisconnected : INotification
{
  public ReaderDisconnected(uint deviceID)
  {
    DeviceID = deviceID;
  }

  public uint DeviceID { get; }
}
