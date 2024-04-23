namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderRegistered : INotification
{
  private readonly ReaderDefinition readerDefinition;

  public ReaderRegistered(ReaderDefinition readerDefinition)
  {
    this.readerDefinition = readerDefinition;
  }

  public uint DeviceID => this.readerDefinition.DeviceID;
  public string ReaderType => this.readerDefinition.ReaderModule.info().readerTypeToString();
}
