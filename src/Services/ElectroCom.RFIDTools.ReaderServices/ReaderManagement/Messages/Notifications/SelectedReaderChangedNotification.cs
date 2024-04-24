namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class SelectedReaderChanged : INotification
{
  private readonly ReaderDefinition readerDefinition;
  public SelectedReaderChanged(ReaderDefinition readerDefinition)
  {
    this.readerDefinition = readerDefinition;
  }

  public uint DeviceName => this.readerDefinition.DeviceID;
  public string ReaderName => this.readerDefinition.DeviceName;
  public bool IsConnected => this.readerDefinition.IsConnected;
}
