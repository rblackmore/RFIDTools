namespace OBID.Scratch.ReaderManagement;

using MediatR;

using OBID.Scratch.ReaderManagement.Model;

public class ReaderRegistered : INotification
{
  public ReaderRegistered(ReaderDefinition readerDefinition)
  {
    ReaderDefinition = readerDefinition;
  }

  public ReaderDefinition ReaderDefinition { get; }
}
