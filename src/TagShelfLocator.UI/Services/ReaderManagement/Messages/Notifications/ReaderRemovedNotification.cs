namespace TagShelfLocator.UI.Services.ReaderManagement;

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
