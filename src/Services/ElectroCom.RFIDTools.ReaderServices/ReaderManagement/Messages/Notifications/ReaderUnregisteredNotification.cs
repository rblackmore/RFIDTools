namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderUnregistered : INotification
{
  public ReaderUnregistered(ReaderDefinition readerDefinition)
  {
    ReaderDefinition = readerDefinition;
  }

  public ReaderDefinition ReaderDefinition { get; }
}
