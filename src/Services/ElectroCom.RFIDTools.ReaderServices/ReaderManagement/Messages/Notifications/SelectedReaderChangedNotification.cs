namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class SelectedReaderChanged : INotification
{
  public SelectedReaderChanged(ReaderDefinition readerDefinition)
  {
    ReaderDefinition = readerDefinition;
  }

  public ReaderDefinition ReaderDefinition { get; }
}
