namespace TagShelfLocator.UI.Services.ReaderManagement.Messages;

using System.Threading.Tasks;

public class ReaderDisconnected
{
  public ReaderDisconnected(uint deviceID)
  {
    DeviceID = deviceID;
  }

  public uint DeviceID { get; }
}
