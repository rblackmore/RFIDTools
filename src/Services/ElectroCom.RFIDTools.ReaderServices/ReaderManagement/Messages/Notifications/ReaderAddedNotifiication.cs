namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using MediatR;

public class ReaderAdded : INotification
{
  private readonly uint deviceId;

  public ReaderAdded(uint deviceId)
  {
    this.deviceId = deviceId;
  }

  public uint DeviceID => this.deviceId;
}
