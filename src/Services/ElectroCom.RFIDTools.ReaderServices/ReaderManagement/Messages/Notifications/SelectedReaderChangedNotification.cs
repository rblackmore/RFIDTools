namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class SelectedReaderChanged : INotification
{
  private readonly ReaderDefinition readerDefinition;
  public SelectedReaderChanged(ReaderDefinition readerDefinition)
  {
    this.readerDefinition = readerDefinition;
  }

  public string ReaderType => this.readerDefinition.ReaderModule.info().readerTypeToString();
  public uint DeviceID => this.readerDefinition.DeviceID;
}
