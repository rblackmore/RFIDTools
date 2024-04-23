namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderRegistered : INotification
{
  public ReaderRegistered(ReaderDefinition readerDefinition)
  {
    ReaderDefinition = readerDefinition;
  }

  public ReaderDefinition ReaderDefinition { get; }
}
