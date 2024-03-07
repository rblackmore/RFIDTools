namespace TagShelfLocator.UI.Services.ReaderConnectionListenerService.Messages;

using System.Threading.Tasks;

public class ReaderDisconnected
{
  public ReaderDisconnected(uint deviceID)
  {
    this.DeviceID = deviceID;
  }

  public uint DeviceID { get; }
}
