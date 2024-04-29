namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderConnected : INotification
{
  public ReaderConnected(ReaderDefinition readerDefinition)
  {
    ReaderDefinition = readerDefinition;
  }

  public uint DeviceID => this.ReaderDefinition.DeviceID;
  public string DeviceName => this.ReaderDefinition.DeviceName;

  public ReaderDefinition ReaderDefinition { get; set; }
}
