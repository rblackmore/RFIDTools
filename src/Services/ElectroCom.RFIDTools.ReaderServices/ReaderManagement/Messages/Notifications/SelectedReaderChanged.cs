namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using MediatR;

public class SelectedReaderChanged : INotification
{
  public SelectedReaderChanged(uint deviceID)
  {
    DeviceID = deviceID;
  }

  public uint DeviceID { get; }
}
