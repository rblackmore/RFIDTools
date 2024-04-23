namespace OBID.Scratch.ReaderManagement;

using MediatR;

using OBID.Scratch.ReaderManagement.Model;

public class ReaderUnregistered : INotification
{
  public ReaderUnregistered(ReaderDefinition readerDefinition)
  {
    ReaderDefinition = readerDefinition;
  }

  public ReaderDefinition ReaderDefinition { get; }
}
