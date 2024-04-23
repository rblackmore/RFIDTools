namespace OBID.Scratch.ReaderManagement;

using MediatR;

using OBID.Scratch.ReaderManagement.Model;

public class SelectedReaderChanged : INotification
{
  public SelectedReaderChanged(ReaderDefinition readerDefinition)
  {
    ReaderDefinition = readerDefinition;
  }

  public ReaderDefinition ReaderDefinition { get; }
}
