namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderUnregistered : INotification
{
  private readonly ReaderDefinition readerDefinition;

  public ReaderUnregistered(ReaderDefinition readerDefinition)
  {
    this.readerDefinition = readerDefinition;
  }

  public uint DeviceID => this.readerDefinition.DeviceID;
  public string ReaderType => this.readerDefinition.ReaderModule.info().readerTypeToString();
}
