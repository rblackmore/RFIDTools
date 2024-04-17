namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using MediatR;

public class ReaderRemoved : INotification
{
  private readonly uint deviceId;

  public ReaderRemoved(uint deviceId)
  {
    this.deviceId = deviceId;
  }

  public uint DeviceID => this.deviceId;
}
